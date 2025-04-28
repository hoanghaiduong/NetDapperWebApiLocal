
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IRoleService
    {
         Task<List<Role>> GetRoles();
    }
}