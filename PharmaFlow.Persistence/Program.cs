using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Interfaces;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Infrastructure.Repositories;
using PharmaFlow.Application.Services;
using PharmaFlow.Persistence.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env (Seguridad)
DotNetEnv.Env.Load("../.env");
builder.Configuration.AddEnvironmentVariables();

// Registrar DbContext con PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PharmaFlowDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar Repositorios y Unit of Work
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar Servicios de Aplicación
builder.Services.AddScoped<IClienteService, ClienteService>();

// Registrar Controladores
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware global de excepciones
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
