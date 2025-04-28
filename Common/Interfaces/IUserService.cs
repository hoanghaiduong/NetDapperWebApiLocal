

using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;
using NetDapperWebApi_local.Services;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO user);
        Task<User> GetUserById(int id,int depth);
        Task<PaginatedResult<User>> GetAllUsers(PaginationModel pagination);
        Task<User> UpdateUser(int id,UpdateUserDTO user);
        Task<string> DeleteUser(int id);
        Task<bool> DeleteUsers(DeleteUsersDTO userIds);
    }
}