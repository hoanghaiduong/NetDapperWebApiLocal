
using System.Data;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.Services
{
    public class RoleService : IRoleService
    {
        private readonly IDbConnection _db;

        public RoleService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<Role>> GetRoles()
        {
            try
            {
                var roles=(await _db.QueryAsync<Role>("Select * From Roles",commandType:CommandType.Text)).ToList();
                return roles;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}