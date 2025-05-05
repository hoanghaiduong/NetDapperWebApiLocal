using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using FluentValidation;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Services
{
    public class AmenitiesService : IAmenitiesService
    {
        private readonly IDbConnection _db;

        public AmenitiesService(IDbConnection db)
        {
            _db = db;
        }



        public async Task<PaginatedResult<Amenities>> GetAllAmenitiesAsync(PaginationModel dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", dto.PageNumber, DbType.Int32);
            parameters.Add("@PageSize", dto.PageSize, DbType.Int32);
            parameters.Add("@Depth", dto.Depth, DbType.Int32);
            parameters.Add("@Search", dto.Search ?? "", DbType.String);

            using var multi = await _db.QueryMultipleAsync(
                "Amenities_GetAll",
                parameters,
                commandType: CommandType.StoredProcedure);

            // Result set 1: Tổng số bản ghi (TotalCount) – có thể dùng để phân trang, ở đây ta chỉ đọc và bỏ qua.
            var totalCount = await multi.ReadFirstAsync<int>();

            // Result set 2: Danh sách Amenitie (danh sách flat theo phân trang)
            var Amenities = (await multi.ReadAsync<Amenities>()).ToList();



            return new PaginatedResult<Amenities>(Amenities, totalCount, currentPage: dto.PageNumber, pageSize: dto.PageSize);
        }

        public async Task<Amenities?> GetAmenitiesByIdAsync(int id, int depth)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Depth", depth, DbType.Int32);

            using var multi = await _db.QueryMultipleAsync(
                "Amenities_GetById",
                parameters,
                commandType: CommandType.StoredProcedure);

            // Result set 1: Thông tin của Amenitie
            var amenities = await multi.ReadFirstOrDefaultAsync<Amenities>();
            if (amenities == null)
                return null;


            return amenities;
        }

        public async Task<Amenities> CreateAmenitiesAsync(CreateAmenitiesDTO dto)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Name", dto.Name);
                parameters.Add("@Description", dto.Description);
                parameters.Add("@Icon", dto.Icon);


                var amenitie = await _db.QueryFirstOrDefaultAsync<Amenities>(
                    "Amenities_Create",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return amenitie ?? throw new Exception("Không thể tạo amenities.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo danh mục: {ex.Message}");
            }
        }


        public async Task<Amenities> UpdateAmenitiesAsync(int id, UpdateAmenitiesDTO dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Name", dto.Name);
            parameters.Add("@Description", dto.Description);
            parameters.Add("@Icon", dto.Icon);
            var result = await _db.QueryFirstOrDefaultAsync<Amenities>("Amenities_Update", parameters, commandType: CommandType.StoredProcedure);


            return result;
        }

        public async Task<bool> DeleteAmenitiesAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            var rows = await _db.ExecuteAsync("Amenities_Delete", parameters, commandType: CommandType.StoredProcedure);
            return rows != -1;
        }

        public async Task<bool> DeleteAmenityIdsAsync(int[] amenityIds)
        {
            var parameters = new DynamicParameters();


            if (amenityIds.Length > 0)
            {
                string jsonIds = JsonSerializer.Serialize<int[]>(amenityIds);
                parameters.Add("@AmenityIds", jsonIds);
            }
            var result = await _db.ExecuteAsync("Amenities_DeleteMultiple", parameters, commandType: CommandType.StoredProcedure);

            return result == -1;
        }
    }
}