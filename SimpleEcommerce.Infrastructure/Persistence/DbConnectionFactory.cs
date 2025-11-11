using System.Data;
using Npgsql;

namespace SimpleEcommerce.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync();
}

public sealed class NpgsqlConnectionFactory(string cs) : IDbConnectionFactory
{
    private readonly string _cs = cs;

    public async Task<IDbConnection> CreateOpenConnectionAsync()
    {
        var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync();
        return conn;
    }
}