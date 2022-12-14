using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Core.Interfaces;
using StudentManagement.Infrastructure.Data;
using StudentManagement.Infrastructure.Data.Repositories;

namespace StudentManagement.Infrastructure;

public static class ClassFactory
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<StudentDbContext>(c =>
            c.UseSqlServer(configuration.GetConnectionString("StudentManagementDb"), options =>
            {
                options.MigrationsAssembly(typeof(ClassFactory).Assembly.FullName);
                options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: Array.Empty<int>());
            }))
            .AddTransient<StudentContextSeed>()
            .AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
    }
}