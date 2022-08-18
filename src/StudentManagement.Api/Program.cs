using StudentManagement.Application;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

await MigrateAndSeedDatabase(app.Services, app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

static async Task MigrateAndSeedDatabase(IServiceProvider services, ILogger logger)
{
    using var scope = services.CreateScope();
    try
    {
        var seed = scope.ServiceProvider.GetRequiredService<StudentContextSeed>();
        await seed.MigrateDatabase();
        await seed.SeedAsync();
    }
    catch (Exception e)
    {
        logger.LogError(e, "Could not seed the DB");
    }
}