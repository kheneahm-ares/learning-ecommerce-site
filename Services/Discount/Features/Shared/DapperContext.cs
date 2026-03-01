using Dapper;
using Npgsql;

namespace Discount.Features.Shared
{
    public class DapperContext
    {
        public NpgsqlConnection _db { get; set; }

        public DapperContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Discount");
            _db = new NpgsqlConnection(connectionString);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            var result = await _db.ExecuteAsync(sql, param);
            return result;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            var result = await _db.QueryFirstOrDefaultAsync<T>(sql, param);
            return result;
        }
    }
}
