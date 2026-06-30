using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.NameTranslation;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Domain.Ports.Services;
using PharmaFlow.Application.Auth.Handlers;
using PharmaFlow.Application.Caja.Handlers;
using PharmaFlow.Application.Clientes.Handlers;
using PharmaFlow.Application.Compras.Handlers;
using PharmaFlow.Application.Dashboard.Handlers;
using PharmaFlow.Application.Inventario.Handlers;
using PharmaFlow.Application.Productos.Handlers;
using PharmaFlow.Application.Proveedores.Handlers;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Sucursales.Handlers;
using PharmaFlow.Application.Usuarios.Handlers;
using PharmaFlow.Application.Ventas.Handlers;
using PharmaFlow.Infrastructure.Adapters.Repositories.Caja;
using PharmaFlow.Infrastructure.Adapters.Repositories.Auth;
using PharmaFlow.Infrastructure.Adapters.Repositories.Compras;
using PharmaFlow.Infrastructure.Adapters.Repositories.Dashboard;
using PharmaFlow.Infrastructure.Adapters.Repositories.Inventario;
using PharmaFlow.Infrastructure.Adapters.Repositories.Productos;
using PharmaFlow.Infrastructure.Adapters.Repositories.Proveedores;
using PharmaFlow.Infrastructure.Adapters.Repositories.Reportes;
using PharmaFlow.Infrastructure.Adapters.Repositories.Sucursales;
using PharmaFlow.Infrastructure.Adapters.Repositories.Usuarios;
using PharmaFlow.Infrastructure.Adapters.Services;
using PharmaFlow.Infrastructure.Data;
using PharmaFlow.Infrastructure.Repositories.Clientes;
using PharmaFlow.Infrastructure.Repositories.Ventas;


var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5194";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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

// ===============================
// Modulo Auth / Usuarios
// ===============================
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<LogoutHandler>();
builder.Services.AddScoped<ListarUsuariosHandler>();
builder.Services.AddScoped<ObtenerUsuarioPorIdHandler>();
builder.Services.AddScoped<CrearUsuarioHandler>();
builder.Services.AddScoped<ActualizarUsuarioHandler>();

// ===============================
// Modulo Core Business: Clientes (Eds)
// ===============================
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ListarClientesHandler>();
builder.Services.AddScoped<ObtenerClientePorIdHandler>();
builder.Services.AddScoped<CrearClienteHandler>();
builder.Services.AddScoped<ActualizarClienteHandler>();
builder.Services.AddScoped<DesactivarClienteHandler>();

// ===============================
// Modulo Productos
// ===============================
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ListarProductosHandler>();
builder.Services.AddScoped<ObtenerProductoPorIdHandler>();
builder.Services.AddScoped<BuscarProductoPorCodigoHandler>();
builder.Services.AddScoped<CrearProductoHandler>();
builder.Services.AddScoped<ActualizarProductoHandler>();
builder.Services.AddScoped<DesactivarProductoHandler>();

// ===============================
// Modulo Sucursales
// ===============================
builder.Services.AddScoped<ISucursalRepository, SucursalRepository>();
builder.Services.AddScoped<ListarSucursalesHandler>();
builder.Services.AddScoped<ObtenerSucursalPorIdHandler>();
builder.Services.AddScoped<CrearSucursalHandler>();
builder.Services.AddScoped<ActualizarSucursalHandler>();

// ===============================
// Modulo Compras / Proveedores
// ===============================
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<RegistrarCompraHandler>();
builder.Services.AddScoped<ListarComprasHandler>();
builder.Services.AddScoped<ObtenerCompraPorIdHandler>();
builder.Services.AddScoped<RecepcionarCompraHandler>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<RegistrarProveedorHandler>();
builder.Services.AddScoped<ActualizarProveedorHandler>();
builder.Services.AddScoped<DesactivarProveedorHandler>();
builder.Services.AddScoped<ListarProveedoresHandler>();
builder.Services.AddScoped<ObtenerProveedorPorIdHandler>();

// ===============================
// Modulo Inventario
// ===============================
builder.Services.AddScoped<IStockLoteRepository, StockLoteRepository>();
builder.Services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();
builder.Services.AddScoped<IInventarioReader, InventarioReader>();
builder.Services.AddScoped<AjustarStockHandler>();
builder.Services.AddScoped<ObtenerStockPorSucursalHandler>();

// ===============================
// Modulo Caja
// ===============================
builder.Services.AddScoped<ICajaRepository, CajaRepository>();
builder.Services.AddScoped<AbrirCajaHandler>();
builder.Services.AddScoped<CerrarCajaHandler>();
builder.Services.AddScoped<RegistrarMovimientoCajaHandler>();
builder.Services.AddScoped<ObtenerTurnoCajaActualHandler>();
builder.Services.AddScoped<ObtenerDetalleTurnoCajaHandler>();

// ===============================
// Modulo Core Business: Ventas (Eds)
// ===============================
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<ListarVentasHandler>();
builder.Services.AddScoped<ObtenerVentaPorIdHandler>();
builder.Services.AddScoped<CrearVentaHandler>();
builder.Services.AddScoped<AnularVentaHandler>();

// ===============================
// Modulo Dashboard / Reportes
// ===============================
builder.Services.AddScoped<IDashboardReader, DashboardReader>();
builder.Services.AddScoped<ObtenerResumenDashboardHandler>();
builder.Services.AddScoped<IReporteVentasReader, ReporteVentasReader>();
builder.Services.AddScoped<ObtenerResumenVentasHandler>();
builder.Services.AddScoped<IReporteInventarioReader, ReporteInventarioReader>();
builder.Services.AddScoped<ObtenerResumenInventarioHandler>();
builder.Services.AddScoped<IReporteCajaReader, ReporteCajaReader>();
builder.Services.AddScoped<ObtenerResumenCajaHandler>();

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
