using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleEcommerce.Application.Abstractions.Auth;
using SimpleEcommerce.Domain.Abstractions.Security;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Infrastructure.Persistence;
using SimpleEcommerce.Infrastructure.Persistence.TypeHandlers;
using SimpleEcommerce.Infrastructure.Repository;
using SimpleEcommerce.Infrastructure.Security;
using SimpleEcommerce.Infrastructure.Services;

namespace SimpleEcommerce.Infrastructure;

public static class DependencyInjection
{
    private static bool _dapperConfigured;

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default") ?? throw new ArgumentNullException("DB connection not defined yet");

        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(cs));

        if (!_dapperConfigured)
        {
            SqlMapper.AddTypeHandler(new EmailTypeHandler());
            SqlMapper.AddTypeHandler(new PasswordTypeHandler());
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _dapperConfigured = true;
        }

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtService>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}