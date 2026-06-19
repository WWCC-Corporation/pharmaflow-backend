using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Reportes.Queries;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Reportes;

public class ReporteInventarioReader : IReporteInventarioReader
{
    private readonly PharmaFlowDbContext dbContext;

    public ReporteInventarioReader(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ResumenInventarioDto> ObtenerResumenAsync(ObtenerResumenInventarioQuery query, CancellationToken cancellationToken)
    {
        var diasVencimiento = query.DiasVencimiento <= 0 ? 30 : query.DiasVencimiento;
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var fechaLimite = hoy.AddDays(diasVencimiento);

        var stockPorProducto = dbContext.VStockPorProductos.AsNoTracking();
        var stockFefo = dbContext.VStockFefos.AsNoTracking();

        if (query.IdSucursal.HasValue)
        {
            stockPorProducto = stockPorProducto.Where(stock => stock.IdSucursal == query.IdSucursal.Value);
            stockFefo = stockFefo.Where(stock => stock.IdSucursal == query.IdSucursal.Value);
        }

        var stockBajoQuery = stockPorProducto
            .Where(stock => stock.StockTotal <= stock.StockMinimo);
        var productosPorVencerQuery = stockFefo
            .Where(stock => stock.StockActual > 0 && stock.FechaVencimiento >= hoy && stock.FechaVencimiento <= fechaLimite);
        var productosVencidosQuery = stockFefo
            .Where(stock => stock.StockActual > 0 && stock.FechaVencimiento < hoy);

        var stockBajo = await stockBajoQuery
            .OrderBy(stock => stock.StockTotal)
            .ThenBy(stock => stock.Nombre)
            .Take(10)
            .Select(stock => new ProductoStockDto
            {
                IdSucursal = stock.IdSucursal,
                Sucursal = stock.Sucursal,
                IdProducto = stock.Id,
                Producto = stock.Nombre,
                CodigoBarra = stock.CodigoBarra,
                StockMinimo = stock.StockMinimo,
                StockTotal = stock.StockTotal,
                Estado = stock.Estado
            })
            .ToListAsync(cancellationToken);

        var productosPorVencer = await productosPorVencerQuery
            .OrderBy(stock => stock.FechaVencimiento)
            .ThenBy(stock => stock.Nombre)
            .Take(10)
            .Select(stock => new ProductoVencimientoDto
            {
                IdSucursal = stock.IdSucursal,
                Sucursal = stock.Sucursal,
                IdProducto = stock.Id,
                Producto = stock.Nombre,
                NumeroLote = stock.NumeroLote,
                FechaVencimiento = stock.FechaVencimiento,
                StockActual = stock.StockActual,
                Estado = stock.Estado
            })
            .ToListAsync(cancellationToken);

        return new ResumenInventarioDto
        {
            IdSucursal = query.IdSucursal,
            ProductosStockBajo = await stockBajoQuery.CountAsync(cancellationToken),
            TotalProductosPorVencer = await productosPorVencerQuery.CountAsync(cancellationToken),
            ProductosVencidos = await productosVencidosQuery.CountAsync(cancellationToken),
            StockBajo = stockBajo,
            ProductosPorVencer = productosPorVencer
        };
    }
}
