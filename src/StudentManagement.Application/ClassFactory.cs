using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Services;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.Application;

public static class ClassFactory
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IStudentService, StudentService>()
            .AddTransient<ISchoolService, SchoolService>();
    }
}