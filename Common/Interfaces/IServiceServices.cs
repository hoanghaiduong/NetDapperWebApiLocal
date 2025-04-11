using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Common.Interfaces
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