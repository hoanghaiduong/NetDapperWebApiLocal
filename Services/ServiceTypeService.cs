using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IDbConnection _db;
        private readonly ILogger<ServiceTypeService> _logger;

        public ServiceTypeService(IDbConnection db, ILogger<ServiceTypeService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ServiceType> CreateServiceType(ServiceTypeDTO dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", dto.Name);
            var exec = await _db.QueryFirstOrDefaultAsync<ServiceType>("ServiceTypes_Create", parameters, commandType: CommandType.StoredProcedure);
            if (exec == null) throw new Exception();
            return exec;
        }

        public async Task<bool> DeleteServiceTypeById(int Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            var exec = await _db.ExecuteAsync("ServiceTypes_Delete", parameters, commandType: CommandType.StoredProcedure);

            return exec == -1;
        }

        public async Task<bool> DeleteServiceTypesAsync(int[] ids)
        {
            var parameters = new DynamicParameters();


            if (ids.Length > 0)
            {
                string jsonIds = JsonSerializer.Serialize<int[]>(ids);
                parameters.Add("@ServiceTypeIds", jsonIds);
            }
            var result = await _db.ExecuteAsync("ServiceTypes_DeleteMultiple", parameters, commandType: CommandType.StoredProcedure);

            return result == -1;
        }

        public async Task<PaginatedResult<ServiceType>> GetAllServiceType(PaginationModel dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", dto.PageNumber);
            parameters.Add("@PageSize", dto.PageSize);
            parameters.Add("@Depth", dto.Depth);
            parameters.Add("@Search", dto.Search);

            var multi = await _db.QueryMultipleAsync("ServiceTypes_GetAll", parameters, commandType: CommandType.StoredProcedure);
            var totalCount = await multi.ReadSingleOrDefaultAsync<int>();
            var serviceTypes = (await multi.ReadAsync<ServiceType>()).ToList();
            return new PaginatedResult<ServiceType>(serviceTypes, totalCount, dto.PageNumber, dto.PageSize);
        }

        public async Task<ServiceType> GetServiceTypeById(int Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            var exec = await _db.QueryFirstOrDefaultAsync<ServiceType>("ServiceTypes_GetById", parameters, commandType: CommandType.StoredProcedure);
            if (exec == null) throw new Exception();
            return exec;
        }

        public async Task<ServiceType> GetServiceTypeByName(ServiceTypeDTO dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", dto.Name);
            var exec = await _db.QueryFirstOrDefaultAsync<ServiceType>("ServiceTypes_GetByName", parameters, commandType: CommandType.StoredProcedure);
            if (exec == null) throw new Exception();
            return exec;
        }

        public async Task<ServiceType> UpdateServiceType(int id, ServiceTypeDTO dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Name", dto.Name);
            var exec = await _db.QueryFirstOrDefaultAsync<ServiceType>("ServiceTypes_Update", parameters, commandType: CommandType.StoredProcedure);
            if (exec == null) throw new Exception();
            return exec;
        }

    }
}