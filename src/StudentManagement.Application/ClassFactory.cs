using Microsoft.Extensions.DependencyInjection;

namespace StudentManagement.Application;

public static class ClassFactory
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}