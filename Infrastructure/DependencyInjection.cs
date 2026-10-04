using Application.Abstractions.Users;
using Application.Abstractions.Reporters;
using Application.Abstractions.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TicketDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TicketDb"),
                sql => sql.MigrationsAssembly(typeof(TicketDbContext).Assembly.GetName().Name)));

        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException(
                $"The '{JwtOptions.SectionName}' configuration section is required.");

        if (string.IsNullOrWhiteSpace(jwtOptions.Issuer)
            || string.IsNullOrWhiteSpace(jwtOptions.Audience)
            || string.IsNullOrWhiteSpace(jwtOptions.SigningKey)
            || Encoding.UTF8.GetByteCount(jwtOptions.SigningKey) < 32
            || jwtOptions.AccessTokenMinutes is < 1 or > 1440)
        {
            throw new InvalidOperationException(
                $"Configure '{JwtOptions.SectionName}' with an issuer, audience, a signing key of at least 32 bytes, and an access token lifetime from 1 to 1440 minutes.");
        }

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReporterRepository, ReporterRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddSingleton<IUserPasswordHasher, IdentityUserPasswordHasher>();
        services.AddSingleton<IUserTokenIssuer, JwtUserTokenIssuer>();

        return services;
    }
}
