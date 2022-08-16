using System.Reflection;
using StudentDataImporter.Api.DataAccess.Repositories;

namespace StudentDataImporter.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
    {
        var concreteTypes = assembly.GetTypes()
            .Where(type => type.ImplementsGenericInterface(typeof(IRepository<>)));
        foreach (var concreteType in concreteTypes)
        {
            services.AddScoped(concreteType
                .GetInterfaces(false).First(), concreteType);
        }

        return services;
    }
}
