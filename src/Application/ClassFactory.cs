using Application.Services;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ClassFactory
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return services.AddScoped<IDataImporterService, DataImporterService>();
    }
}