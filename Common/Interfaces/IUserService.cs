

using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using NetDapperWebApi.Services;

namespace NetDapperWebApi.Common.Interfaces
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