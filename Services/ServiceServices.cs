using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Services
{
    public class ServiceServices : IServiceServices
    {
        private readonly IDbConnection _db;
        private readonly ILogger<ServiceServices> _logger;

        public ServiceServices(IDbConnection db, ILogger<ServiceServices> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Service> CreateService(CreateServiceDTO dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Name", dto.Name);
            parameters.Add("@Description", dto.Description);
            parameters.Add("@Price", dto.Price);
            parameters.Add("@ServiceTypeId", dto.ServiceTypeId);
            parameters.Add("@CreatedAt", DateTime.Now);
            parameters.Add("@UpdatedAt", DateTime.Now);
            var result = await _db.QueryFirstOrDefaultAsync<Service>("Services_Create", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> DeleteService(int id)
        {
            //parameters
            // @Id INT

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var result = await _db.ExecuteAsync("Services_Delete", parameters, commandType: CommandType.StoredProcedure);
            return result == -1;
        }

        public async Task<bool> DeleteServices(int[] ids)
        {
            var parameters = new DynamicParameters();


            if (ids.Length > 0)
            {
                string jsonIds = JsonSerializer.Serialize<int[]>(ids);
                parameters.Add("@ServiceIds", jsonIds);
            }
            var result = await _db.ExecuteAsync("Services_DeleteMultiple", parameters, commandType: CommandType.StoredProcedure);

            return result == -1;
        }

        public async Task<Service> GetServiceById(int id, int depth)
        {
            //parameters 
            //@Id
            //@Depth
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);
            using var multi = await _db.QueryMultipleAsync("Services_GetById", parameters, commandType: CommandType.StoredProcedure);
            var service = await multi.ReadSingleOrDefaultAsync<Service>();

            if (depth >= 1)
            {
                var serviceUsages = (await multi.ReadAsync<ServiceUsage>()).ToList();
                service.ServiceUsages = [.. serviceUsages.Where(x => x.ServiceId == service.Id)];
            }
            return service;
        }

        public async Task<PaginatedResult<Service>> GetServices(PaginationModel dto)
        {
            //parameters
            //         @PageNumber INT = 1,
            // @PageSize INT = 10,
            // @Depth INT = 0,
            // @Search VARCHAR(255) = ''
            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", dto.PageNumber);
            parameters.Add("@PageSize", dto.PageSize);
            parameters.Add("@Depth", dto.Depth);
            parameters.Add("@Search", dto.Search);
            using var result = await _db.QueryMultipleAsync("Services_GetAll", parameters, commandType: CommandType.StoredProcedure);
            var total = await result.ReadFirstOrDefaultAsync<int>();
            var services = (await result.ReadAsync<Service>()).ToList();
            if (dto.Depth >= 1)
            {
                var serviceUsages = await result.ReadAsync<ServiceUsage>();
                services.ForEach(x => x.ServiceUsages = serviceUsages.Where(y => y.ServiceId == x.Id).ToList());
            }

            // services.ForEach(x => x.ServiceUsages = serviceUsages.Where(y => y.ServiceId == x.Id).ToList());

            return new PaginatedResult<Service>(services, total, dto.PageNumber, dto.PageSize);
        }

        public Task<Service> UpdateService(int id, UpdateServiceDTO dto)
        {
            //params
            // @Id INT,
            // @Name NVARCHAR(255) = NULL,
            // @Description NVARCHAR(500) = NULL,
            // @Price DECIMAL(18,2) = NULL,
            // @UpdatedAt DATETIME2(7) = NULL
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Name", dto.Name);
            parameters.Add("@Description", dto.Description);
            parameters.Add("@Price", dto.Price);
            if (dto.ServiceTypeId != null)
            {
                parameters.Add("@ServiceTypeId", dto.ServiceTypeId);
            }

            parameters.Add("@UpdatedAt", DateTime.Now);
            var result = _db.QueryFirstOrDefaultAsync<Service>("Services_Update", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}