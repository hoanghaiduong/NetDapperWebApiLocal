
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IRoleService
    {
         Task<List<Role>> GetRoles();
    }
}