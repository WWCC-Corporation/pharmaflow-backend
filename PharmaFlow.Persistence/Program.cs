using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Common.Behaviors;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Handlers.Compras;
using PharmaFlow.Application.Features.Reportes.Handlers;
using PharmaFlow.Application.Interfaces;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Infrastructure.Repositories;
using PharmaFlow.Infrastructure.Repositories.Compras;
using PharmaFlow.Persistence.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env.
// Se intenta cargar desde la raíz del proyecto o desde la carpeta anterior,
// dependiendo de cómo se ejecute la API.
var envPathRoot = Path.Combine(Directory.GetCurrentDirectory(), ".env");
var envPathParent = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");

if (File.Exists(envPathRoot))
{
    DotNetEnv.Env.Load(envPathRoot);
}
else if (File.Exists(envPathParent))
{
    DotNetEnv.Env.Load(envPathParent);
}

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Registrar Base de Datos
// Se mantiene la configuración base actualizada por Diego.
builder.Services.AddDbContext<PharmaFlowDbContext>();

// 2. Registrar Unit of Work y Repositorios generales
// Configuración base de arquitectura agregada por Diego.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. Registrar Handlers y Servicios de reportes
// Configuración base de reportes agregada por Diego.
builder.Services.AddScoped<ObtenerReporteVentasHandler>();

// ===============================
// CAMBIO ALEXANDRO: Módulo Abastecimiento / Compras (CQRS con MediatR y FluentValidation)
// Se registran:
//  - MediatR: descubre Commands, Queries y Handlers del módulo.
//  - FluentValidation: descubre los Validators del módulo.
//  - ValidationBehavior: pipeline que valida cada Command/Query antes del Handler.
//  - Repositorios concretos (Infrastructure) que implementan los contratos de Application.
// ===============================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CrearCompraHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CrearCompraHandler).Assembly);

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();

var app = builder.Build();

// Configurar el Middleware Global de Excepciones ANTES de cualquier otra cosa.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// ===============================
// CAMBIO ALEXANDRO: Mapeo de Controllers
// Permite que la API reconozca rutas como:
// GET /api/proveedores
// POST /api/compras
// ===============================
app.MapControllers();

app.Run();