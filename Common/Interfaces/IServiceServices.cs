using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IServiceServices
    {
        Task<Service> CreateService(CreateServiceDTO dto);
        Task<Service> GetServiceById(int id,int depth);
        Task<PaginatedResult<Service>> GetServices(PaginationModel dto);
        Task<Service> UpdateService(int id, UpdateServiceDTO dto);
        Task<int> DeleteService(int id);

    }
}