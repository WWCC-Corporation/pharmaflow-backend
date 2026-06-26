using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.NameTranslation;
using PharmaFlow.Application.Dashboard.Handlers;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Compras.Handlers;
using PharmaFlow.Application.Proveedores.Handlers;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Adapters.Repositories.Compras;
using PharmaFlow.Infrastructure.Adapters.Repositories.Dashboard;
using PharmaFlow.Infrastructure.Adapters.Repositories.Proveedores;
using PharmaFlow.Infrastructure.Adapters.Repositories.Reportes;
using PharmaFlow.Infrastructure.Data;

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
    options.UseNpgsql(dataSource, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
        npgsqlOptions.CommandTimeout(120);
    }));
builder.Services.AddScoped<IDashboardReader, DashboardReader>();
builder.Services.AddScoped<ObtenerResumenDashboardHandler>();
builder.Services.AddScoped<IReporteVentasReader, ReporteVentasReader>();
builder.Services.AddScoped<ObtenerResumenVentasHandler>();
builder.Services.AddScoped<IReporteInventarioReader, ReporteInventarioReader>();
builder.Services.AddScoped<ObtenerResumenInventarioHandler>();
builder.Services.AddScoped<IReporteCajaReader, ReporteCajaReader>();
builder.Services.AddScoped<ObtenerResumenCajaHandler>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<RegistrarProveedorHandler>();
builder.Services.AddScoped<ActualizarProveedorHandler>();
builder.Services.AddScoped<DesactivarProveedorHandler>();
builder.Services.AddScoped<ListarProveedoresHandler>();
builder.Services.AddScoped<ObtenerProveedorPorIdHandler>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<RegistrarCompraHandler>();
builder.Services.AddScoped<ListarComprasHandler>();
builder.Services.AddScoped<ObtenerCompraPorIdHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
