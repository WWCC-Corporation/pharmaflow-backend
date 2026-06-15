using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using PharmaFlow.Application.Common.Behaviors;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Handlers.Compras;
=======
using Npgsql;
using Npgsql.NameTranslation;
using PharmaFlow.Domain.Enums;
>>>>>>> origin/dev
using PharmaFlow.Application.Features.Reportes.Handlers;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Infrastructure.Repositories.Reportes;

var builder = WebApplication.CreateBuilder(args);

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'ConnectionStrings__DefaultConnection'.");

var enumNameTranslator = new NpgsqlNullNameTranslator();
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<EstadoCompra>("estado_compra", enumNameTranslator);
dataSourceBuilder.MapEnum<EstadoVenta>("estado_venta", enumNameTranslator);
dataSourceBuilder.MapEnum<MetodoPago>("metodo_pago", enumNameTranslator);
dataSourceBuilder.MapEnum<Moneda>("moneda", enumNameTranslator);
dataSourceBuilder.MapEnum<TipoAlerta>("tipo_alerta", enumNameTranslator);
dataSourceBuilder.MapEnum<TipoMovimiento>("tipo_movimiento", enumNameTranslator);
dataSourceBuilder.MapEnum<TipoMovimientoCaja>("tipo_movimiento_caja", enumNameTranslator);
var dataSource = dataSourceBuilder.Build();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddSingleton(dataSource);
builder.Services.AddDbContext<PharmaFlowDbContext>(options =>
    options.UseNpgsql(dataSource));
builder.Services.AddScoped<IReporteVentasReader, ReporteVentasReader>();
builder.Services.AddScoped<ObtenerResumenVentasHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
<<<<<<< HEAD

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
=======
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});
>>>>>>> origin/dev

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("DefaultCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
