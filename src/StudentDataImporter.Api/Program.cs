using StudentDataImporter.Api;
using StudentDataImporter.Api.Configuration;
using StudentDataImporter.Api.DataAccess.Entities;
using StudentDataImporter.Api.DataAccess.Repositories;
using StudentDataImporter.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register app services
builder.Services.AddTransient<IDataImporter, DataImporter>();
builder.Services.AddSingleton(builder.Configuration.GetRequiredSection("ConfigurationConstants").Get<ConfigurationConstants>());
builder.Services.AddScoped<IRepository<District>, DistrictRepository>();
builder.Services.AddScoped<IRepository<School>, SchoolRepository>();
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<CourseEnrollment>, CourseEnrollmentRepository>();
builder.Services.AddScoped<IRepository<SchoolEnrollment>, SchoolEnrollmentRepository>();
builder.Services.AddScoped<IRepository<CourseGrade>, CourseGradeRepository>();
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddTransient<IRowParser, RowParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
