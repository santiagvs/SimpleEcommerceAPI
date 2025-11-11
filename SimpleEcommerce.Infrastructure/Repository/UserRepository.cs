using Dapper;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Infrastructure.Persistence;

namespace SimpleEcommerce.Infrastructure.Repository;

public sealed class UserRepository(IDbConnectionFactory factory) : IUserRepository
{
    private readonly IDbConnectionFactory _factory = factory;

    public async Task AddAsync(User user)
    {
        const string sql = """
        INSERT INTO users (id, email, password_hash, first_name, last_name, role, created_at, updated_at)
        VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @Role, @CreatedAt, @UpdatedAt);
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        await conn.ExecuteAsync(sql, new
        {
            user.Id,
            Email = user.Email.Address,
            PasswordHash = user.Password.Hash,
            user.FirstName,
            user.LastName,
            Role = (int)user.Role,
            user.CreatedAt,
            user.UpdatedAt,
        });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = """
        SELECT
            id,
            email,
            password_hash AS Password,
            first_name AS FirstName,
            last_name AS LastName,
            role AS Role,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM users
        WHERE id = @id;
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """
        SELECT
            id,
            email,
            password_hash AS Password,
            first_name AS FirstName,
            last_name AS LastName,
            role AS Role,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
        FROM users
        WHERE email = @email;
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { email });
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE email = @email);";
        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.ExecuteScalarAsync<bool>(sql, new { email });
    }
}