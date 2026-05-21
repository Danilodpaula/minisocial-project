using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSocial.Infrastructure.Data;

namespace MiniSocial.Infrastructure;

public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MiniSocialDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
}