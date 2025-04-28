

using System.Data;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;
using Newtonsoft.Json;

namespace NetDapperWebApi_local.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDbConnection _db;

        private readonly IFileUploadService _fileService;
        public UserService(ILogger<UserService> logger, IDbConnection db, IFileUploadService fileService)
        {
            _logger = logger;
            _db = db;

            _fileService = fileService;
        }

        public async Task<User> CreateUser(CreateUserDTO user)
        {
            var newAvatar = string.Empty;
            try
            {
                if (user.Avatar != null)
                {
                    newAvatar = await _fileService.UploadSingleFile(new[] { "uploads", "images", "users", "avatars" }, user.Avatar);
                }
                var parameters = new DynamicParameters();
                parameters.Add("@Email", user.Email);
                parameters.Add("@Password", user.Password);
                parameters.Add("@PhoneNumber", user.PhoneNumber);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@EmailVerified", user.EmailVerified);
                parameters.Add("@Avatar", newAvatar);
                parameters.Add("@RefreshToken", user.RefreshToken);
                parameters.Add("@IsDisabled", user.IsDisabled);
                parameters.Add("@LastLogin", user.LastLogin);
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    var roleIds = Newtonsoft.Json.JsonConvert.SerializeObject(user.Roles);
                    parameters.Add("@RolesJson", roleIds);
                }

                // Gọi stored procedure Users_Create
                var result = await _db.QueryFirstOrDefaultAsync<User>(
                    "Users_Create", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (System.Exception)
            {
                if (!string.IsNullOrEmpty(newAvatar))
                {
                    _fileService.DeleteSingleFile(newAvatar);
                }

                throw;
            }

        }

        public async Task<string> DeleteUser(int id)
        {
            
            var user = await GetUserById(id, 0) ?? throw new Exception("Not Found");
            string avatar = user.Avatar!;
            if (!string.IsNullOrEmpty(avatar))
            {
                 _fileService.DeleteSingleFile(avatar);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var result = await _db.QueryFirstOrDefaultAsync<string>(
                "Users_Delete", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> DeleteUsers(DeleteUsersDTO dto)
        {
            try
            {
                if (dto.UserIds == null || dto.UserIds.Count == 0)
                    return false;

                var parameters = new DynamicParameters();
                parameters.Add("@UserIds", JsonConvert.SerializeObject(dto.UserIds));

                using var multi = await _db.QueryMultipleAsync(
                    "Users_DeleteMultiple", parameters, commandType: CommandType.StoredProcedure);

                var avatars = (await multi.ReadAsync<string>())
                .Where(a => !string.IsNullOrWhiteSpace(a)) // lọc avatar hợp lệ
                .ToList();



                return _fileService.DeleteMultipleFiles(avatars);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa người dùng");
                return false;
            }
        }


        // Chỉ lấy danh sách relation cấp 1
        public async Task<PaginatedResult<User>> GetAllUsers(PaginationModel paginationModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", paginationModel.PageNumber);
            parameters.Add("@PageSize", paginationModel.PageSize);
            parameters.Add("@Depth", paginationModel.Depth);
            parameters.Add("@Search", paginationModel.Search);

            int totalCount;
            List<User>? userRelations;

            using var multi = await _db.QueryMultipleAsync(
                "Users_GetAll", parameters, commandType: CommandType.StoredProcedure);

            // Đọc tổng số lượng user
            totalCount = await multi.ReadSingleAsync<int>();

            // Đọc danh sách user
            userRelations = (await multi.ReadAsync<User>()).ToList();//user with relations

            if (paginationModel.Depth >= 1)
            {


                var roles = (await multi.ReadAsync<Role>()).ToList();

                // Đọc danh sách bookings
                var bookings = (await multi.ReadAsync<Booking>()).ToList();
                foreach (var user in userRelations)
                {


                    user.Roles = [.. roles.Where(s => s.UserId == user.Id)];

                    user.Bookings = [.. bookings.Where(b => b.UserId == user.Id)];
                }
            }


            return new PaginatedResult<User>(userRelations, totalCount, paginationModel.PageNumber, paginationModel.PageSize);
        }


        public async Task<User> GetUserById(int id, int depth)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);

            User? user;

            using (var multi = await _db.QueryMultipleAsync(
                "Users_GetByID", parameters, commandType: CommandType.StoredProcedure))
            {
                user = await multi.ReadSingleOrDefaultAsync<User>();


                if (depth == 1 && user != null)
                {

                    user.Roles = [.. (await multi.ReadAsync<Role>()).ToList().Where(s => s.UserId == user.Id)];
                    user.Bookings = (await multi.ReadAsync<Booking>()).ToList();
                }
            }
            return user;
        }

        public async Task<User> UpdateUser(int id, UpdateUserDTO user)
        {
            string? newAvatar = null;
            try
            {
                var currentUser = await GetUserById(id, 0);

                if (user.Avatar != null)
                {
                    // Chỉ xóa ảnh cũ nếu có ảnh mới hợp lệ
                    newAvatar = await _fileService.UploadSingleFile(["uploads", "images", "users", "avatars"], user.Avatar);
                    if (!string.IsNullOrEmpty(newAvatar) && !string.IsNullOrEmpty(currentUser.Avatar))
                    {
                        _fileService.DeleteSingleFile(currentUser.Avatar);
                        currentUser.Avatar = newAvatar;
                    }
                }

                var parameters = new DynamicParameters(new
                {
                    Id = id,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                    user.EmailVerified,
                    Avatar = newAvatar ?? currentUser.Avatar,
                    user.RefreshToken,
                    user.IsDisabled,
                    user.LastLogin,
                });
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    var roleIds = Newtonsoft.Json.JsonConvert.SerializeObject(user.Roles);
                    parameters.Add("@RolesJson", roleIds);
                }
                var multi = await _db.QueryMultipleAsync("Users_Update", parameters, commandType: CommandType.StoredProcedure);
                return await multi.ReadSingleAsync<User>();
            }
            catch (SqlException ex)
            {
                // Log lỗi database cụ thể
                throw new Exception("Lỗi khi cập nhật user vào database.", ex);
            }
            catch (IOException ex)
            {
                // Log lỗi file hệ thống
                throw new Exception("Lỗi khi xử lý ảnh đại diện.", ex);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(newAvatar))
                {
                    _fileService.DeleteSingleFile(newAvatar);
                }
                throw new Exception("Lỗi không xác định khi cập nhật user.", ex);
            }
        }



    }
}