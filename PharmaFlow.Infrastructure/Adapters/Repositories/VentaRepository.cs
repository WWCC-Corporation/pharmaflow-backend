using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Ventas.Handlers;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Persistence;

namespace PharmaFlow.Infrastructure.Repositories.Ventas;

public class VentaRepository : IVentaRepository
{
    private readonly PharmaFlowDbContext context;

    public VentaRepository(PharmaFlowDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Venta>> ListarAsync(CancellationToken cancellationToken)
    {
        return await context.Ventas
            .AsNoTracking()
            .Include(v => v.DetalleVenta)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task<Venta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Ventas
            .AsNoTracking()
            .Include(v => v.DetalleVenta)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Venta> CrearAsync(Venta venta, CancellationToken cancellationToken)
    {
        ValidarDatosCreacion(venta);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        venta.Id = Guid.NewGuid();
        venta.Estado = EstadoVenta.completada;
        venta.Fecha = DateTime.UtcNow;
        if (!venta.TipoCambio.HasValue || venta.TipoCambio == 0)
        {
            venta.TipoCambio = 1;
        }

        foreach (var detalle in venta.DetalleVenta)
        {
            ValidarDetalle(detalle);

            detalle.Id = Guid.NewGuid();
            detalle.IdVenta = venta.Id;

            if (detalle.IdLote.HasValue)
            {
                await DescontarStockAsync(
                    detalle.IdLote.Value,
                    detalle.IdProducto,
                    detalle.Cantidad,
                    venta,
                    detalle,
                    cancellationToken);
            }
        }

        context.Ventas.Add(venta);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return venta;
    }

    public async Task<bool> AnularAsync(Guid id, CancellationToken cancellationToken)
    {
        var venta = await context.Ventas
            .Include(v => v.DetalleVenta)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (venta is null)
        {
            return false;
        }

        if (venta.Estado == EstadoVenta.anulada)
        {
            throw new ArgumentException("La venta ya está anulada.");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        venta.Estado = EstadoVenta.anulada;

        foreach (var detalle in venta.DetalleVenta)
        {
            if (!detalle.IdLote.HasValue)
            {
                continue;
            }

            var stock = await context.StockLotes
                .FirstOrDefaultAsync(s => s.IdLote == detalle.IdLote, cancellationToken);

            if (stock is not null)
            {
                stock.StockActual += detalle.Cantidad;
                stock.UpdatedAt = DateTime.UtcNow;
                stock.Version++;
            }

            context.MovimientoInventarios.Add(new MovimientoInventario
            {
                Id = Guid.NewGuid(),
                Tipo = TipoMovimiento.DEVOLUCION,
                IdProducto = detalle.IdProducto,
                IdLote = detalle.IdLote,
                IdVenta = venta.Id,
                IdDetalleVenta = detalle.Id,
                Cantidad = detalle.Cantidad,
                UsuarioId = venta.IdUsuario,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return true;
    }

    private async Task DescontarStockAsync(
        Guid idLote,
        Guid? idProducto,
        int cantidad,
        Venta venta,
        DetalleVenta detalle,
        CancellationToken cancellationToken)
    {
        var stock = await context.StockLotes
            .FirstOrDefaultAsync(s => s.IdLote == idLote, cancellationToken);

        if (stock is null)
        {
            throw new ArgumentException($"No existe stock registrado para el lote {idLote}.");
        }

        if (stock.StockActual < cantidad)
        {
            throw new ArgumentException($"Stock insuficiente para el lote {idLote}.");
        }

        stock.StockActual -= cantidad;
        stock.UpdatedAt = DateTime.UtcNow;
        stock.Version++;

        context.MovimientoInventarios.Add(new MovimientoInventario
        {
            Id = Guid.NewGuid(),
            Tipo = TipoMovimiento.SALIDA,
            IdProducto = idProducto,
            IdLote = idLote,
            IdVenta = venta.Id,
            IdDetalleVenta = detalle.Id,
            Cantidad = cantidad,
            UsuarioId = venta.IdUsuario,
            CreatedAt = DateTime.UtcNow
        });
    }

    private static void ValidarDatosCreacion(Venta venta)
    {
        if (venta.DetalleVenta == null || venta.DetalleVenta.Count == 0)
        {
            throw new ArgumentException("La venta debe tener al menos un detalle.");
        }

        if (venta.MontoTotal <= 0)
        {
            throw new ArgumentException("El monto total debe ser mayor a cero.");
        }
    }

    private static void ValidarDetalle(DetalleVenta detalle)
    {
        if (detalle.Cantidad <= 0)
        {
            throw new ArgumentException("La cantidad del detalle debe ser mayor a cero.");
        }

        if (!detalle.IdProducto.HasValue)
        {
            throw new ArgumentException("Cada detalle debe incluir un producto.");
        }
    }
}
