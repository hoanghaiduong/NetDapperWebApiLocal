using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApi.Context
{
    public class DapperDbContext : IDisposable
    {
        private readonly IDbConnection _connection;

        public DapperDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection Connection => _connection;

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
            _connection.Dispose();
        }
    }
}
