using StudentDataImporter.Api.Configuration;
using StudentDataImporter.Api.Extensions;
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
builder.Services.AddRepositories(typeof(Program).Assembly);
builder.Services.AddTransient<IRowParser, RowParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
