using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ClassFactory
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<StudentDbContext>(c =>
            c.UseSqlServer(configuration.GetConnectionString("StudentDb"), options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: Array.Empty<int>());
            }))
            .AddTransient<StudentContextSeed>();
    }
}