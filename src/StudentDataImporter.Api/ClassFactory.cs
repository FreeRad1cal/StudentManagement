using StudentDataImporter.Api.Services;

namespace StudentDataImporter.Api;

public static class ClassFactory
{
    public static IServiceCollection RegisterImporterServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<IDataImporterService, DataImporterService>();
    }
}
