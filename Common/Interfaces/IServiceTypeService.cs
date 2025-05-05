using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IServiceTypeService
    {
        Task<ServiceType> CreateServiceType(ServiceTypeDTO dto);
        Task<ServiceType> GetServiceTypeByName(ServiceTypeDTO dto);
        Task<ServiceType> GetServiceTypeById(int Id);
        Task<PaginatedResult<ServiceType>> GetAllServiceType(PaginationModel dto);
        Task<ServiceType> UpdateServiceType(int id,ServiceTypeDTO dto);

        Task<bool> DeleteServiceTypeById(int Id);
        Task<bool> DeleteServiceTypesAsync(int[] ids);

    }
}