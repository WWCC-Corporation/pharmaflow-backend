using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Dashboard.DTOs;
using PharmaFlow.Application.Dashboard.Handlers;
using PharmaFlow.Application.Dashboard.Queries;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Dashboard;

public class DashboardReader : IDashboardReader
{
    private const int DiasVencimiento = 30;
    private readonly PharmaFlowDbContext dbContext;

    public DashboardReader(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<DashboardResumenDto> ObtenerResumenAsync(ObtenerResumenDashboardQuery query, CancellationToken cancellationToken)
    {
        var fechaConsulta = query.Fecha?.Date ?? DateTime.Today;
        var inicioDia = fechaConsulta.Date;
        var finDia = inicioDia.AddDays(1);
        var fechaVencimientoInicio = DateOnly.FromDateTime(inicioDia);
        var fechaVencimientoFin = fechaVencimientoInicio.AddDays(DiasVencimiento);

        var ventasDelDia = dbContext.Ventas
            .AsNoTracking()
            .Where(venta => venta.Estado == EstadoVenta.completada)
            .Where(venta => venta.Fecha >= inicioDia && venta.Fecha < finDia);

        var comprasDelDia = dbContext.Compras
            .AsNoTracking()
            .Where(compra => compra.Estado != EstadoCompra.anulada)
            .Where(compra => compra.Fecha >= inicioDia && compra.Fecha < finDia);

        var alertasNoLeidas = dbContext.Alertas
            .AsNoTracking()
            .Where(alerta => !alerta.Leida);

        var stockPorProducto = dbContext.VStockPorProductos
            .AsNoTracking();

        var productosPorVencer = dbContext.VStockFefos
            .AsNoTracking()
            .Where(stock => stock.StockActual > 0)
            .Where(stock => stock.FechaVencimiento >= fechaVencimientoInicio && stock.FechaVencimiento <= fechaVencimientoFin);

        var ventasRecientes = dbContext.Ventas
            .AsNoTracking()
            .Where(venta => venta.Estado == EstadoVenta.completada);

        var comprasRecientes = dbContext.Compras
            .AsNoTracking()
            .Where(compra => compra.Estado != EstadoCompra.anulada);

        var movimientosCajaRecientes = dbContext.MovimientosCajas
            .AsNoTracking();

        if (query.IdSucursal.HasValue)
        {
            ventasDelDia = ventasDelDia.Where(venta => venta.IdSucursal == query.IdSucursal.Value);
            comprasDelDia = comprasDelDia.Where(compra => compra.IdSucursal == query.IdSucursal.Value);
            alertasNoLeidas = alertasNoLeidas.Where(alerta => alerta.IdSucursal == query.IdSucursal.Value);
            stockPorProducto = stockPorProducto.Where(stock => stock.IdSucursal == query.IdSucursal.Value);
            productosPorVencer = productosPorVencer.Where(stock => stock.IdSucursal == query.IdSucursal.Value);
            ventasRecientes = ventasRecientes.Where(venta => venta.IdSucursal == query.IdSucursal.Value);
            comprasRecientes = comprasRecientes.Where(compra => compra.IdSucursal == query.IdSucursal.Value);
            movimientosCajaRecientes = movimientosCajaRecientes.Where(movimiento => movimiento.IdSucursal == query.IdSucursal.Value);
        }

        var totalVendido = await ventasDelDia
            .Select(venta => (decimal?)venta.MontoTotal)
            .SumAsync(cancellationToken) ?? 0;

        return new DashboardResumenDto
        {
            FechaConsulta = fechaConsulta,
            IdSucursal = query.IdSucursal,
            VentasHoy = await ventasDelDia.CountAsync(cancellationToken),
            MontoVendidoHoy = totalVendido,
            ComprasHoy = await comprasDelDia.CountAsync(cancellationToken),
            AlertasNoLeidas = await alertasNoLeidas.CountAsync(cancellationToken),
            ProductosStockBajo = await stockPorProducto
                .Where(stock => stock.StockTotal <= stock.StockMinimo)
                .CountAsync(cancellationToken),
            ProductosPorVencer = await productosPorVencer.CountAsync(cancellationToken),
            VentasRecientes = await ventasRecientes
                .OrderByDescending(venta => venta.Fecha)
                .Take(5)
                .Select(venta => new ActividadRecienteDto
                {
                    Id = venta.Id,
                    Tipo = "venta",
                    Descripcion = "Venta completada",
                    Monto = venta.MontoTotal,
                    Fecha = venta.Fecha
                })
                .ToListAsync(cancellationToken),
            ComprasRecientes = await comprasRecientes
                .OrderByDescending(compra => compra.Fecha)
                .Take(5)
                .Select(compra => new ActividadRecienteDto
                {
                    Id = compra.Id,
                    Tipo = "compra",
                    Descripcion = "Compra " + compra.Estado,
                    Monto = null,
                    Fecha = compra.Fecha
                })
                .ToListAsync(cancellationToken),
            MovimientosCajaRecientes = await movimientosCajaRecientes
                .OrderByDescending(movimiento => movimiento.CreatedAt)
                .Take(5)
                .Select(movimiento => new ActividadRecienteDto
                {
                    Id = movimiento.Id,
                    Tipo = "caja",
                    Descripcion = movimiento.Tipo.ToString(),
                    Monto = movimiento.Monto,
                    Fecha = movimiento.CreatedAt
                })
                .ToListAsync(cancellationToken)
        };
    }
}
