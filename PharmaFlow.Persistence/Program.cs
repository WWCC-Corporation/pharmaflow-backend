var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env (Seguridad)
DotNetEnv.Env.Load("../.env");
builder.Configuration.AddEnvironmentVariables();

using PharmaFlow.Application.Interfaces;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Infrastructure.Repositories;
using PharmaFlow.Persistence.Middlewares;
using PharmaFlow.Application.Features.Reportes.Handlers;
using Microsoft.EntityFrameworkCore;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Registrar Base de Datos
builder.Services.AddDbContext<PharmaFlowDbContext>();

// 2. Registrar Unit of Work y Repositorios (Infraestructura Base)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. Registrar Handlers y Servicios (Ejemplo de Reportes)
builder.Services.AddScoped<ObtenerReporteVentasHandler>();

var app = builder.Build();

// Configurar el Middleware Global de Excepciones ANTES de cualquier otra cosa
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapear los endpoints de los controladores
app.MapControllers();

app.Run();
