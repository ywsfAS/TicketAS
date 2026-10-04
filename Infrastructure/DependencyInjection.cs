using Application.Abstractions.Users;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IUserPasswordHasher, IdentityUserPasswordHasher>();

        return services;
    }
}
