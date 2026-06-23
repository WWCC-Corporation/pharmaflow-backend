using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Features.Ventas.DTOs;
using PharmaFlow.Application.Features.Ventas.Handlers;
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

    public async Task<List<VentaResponseDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var ventas = await context.Ventas
            .AsNoTracking()
            .Include(v => v.DetalleVenta)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync(cancellationToken);

        return ventas.Select(MapToDto).ToList();
    }

    public async Task<VentaResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var venta = await context.Ventas
            .AsNoTracking()
            .Include(v => v.DetalleVenta)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        return venta is null ? null : MapToDto(venta);
    }

    public async Task<VentaResponseDto> CrearAsync(CreateVentaDto dto, CancellationToken cancellationToken)
    {
        ValidarDatosCreacion(dto);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var venta = new Venta
        {
            Id = Guid.NewGuid(),
            Estado = EstadoVenta.completada,
            Moneda = dto.Moneda,
            Metodo = dto.Metodo,
            IdCliente = dto.IdCliente,
            IdUsuario = dto.IdUsuario,
            IdTurnoCaja = dto.IdTurnoCaja,
            Fecha = DateTime.UtcNow,
            TipoCambio = dto.TipoCambio ?? 1,
            MontoTotal = dto.MontoTotal,
            MontoRecibido = dto.MontoRecibido,
            Vuelto = dto.Vuelto
        };

        foreach (var detalleDto in dto.Detalles)
        {
            ValidarDetalle(detalleDto);

            var detalle = new DetalleVenta
            {
                Id = Guid.NewGuid(),
                IdVenta = venta.Id,
                IdLote = detalleDto.IdLote,
                IdProducto = detalleDto.IdProducto,
                Cantidad = detalleDto.Cantidad,
                PrecioUnitario = detalleDto.PrecioUnitario
            };

            venta.DetalleVenta.Add(detalle);

            if (detalleDto.IdLote.HasValue)
            {
                await DescontarStockAsync(
                    detalleDto.IdLote.Value,
                    detalleDto.IdProducto,
                    detalleDto.Cantidad,
                    venta,
                    detalle,
                    cancellationToken);
            }
        }

        context.Ventas.Add(venta);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return MapToDto(venta);
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

    private static void ValidarDatosCreacion(CreateVentaDto dto)
    {
        if (dto.Detalles.Count == 0)
        {
            throw new ArgumentException("La venta debe tener al menos un detalle.");
        }

        if (dto.MontoTotal <= 0)
        {
            throw new ArgumentException("El monto total debe ser mayor a cero.");
        }
    }

    private static void ValidarDetalle(CreateDetalleVentaDto detalle)
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

    private static VentaResponseDto MapToDto(Venta venta)
    {
        return new VentaResponseDto
        {
            Id = venta.Id,
            Estado = venta.Estado,
            Moneda = venta.Moneda,
            Metodo = venta.Metodo,
            IdCliente = venta.IdCliente,
            IdUsuario = venta.IdUsuario,
            IdTurnoCaja = venta.IdTurnoCaja,
            Fecha = venta.Fecha,
            TipoCambio = venta.TipoCambio,
            MontoTotal = venta.MontoTotal,
            MontoRecibido = venta.MontoRecibido,
            Vuelto = venta.Vuelto,
            Detalles = venta.DetalleVenta
                .Select(d => new DetalleVentaResponseDto
                {
                    Id = d.Id,
                    IdVenta = d.IdVenta,
                    IdLote = d.IdLote,
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                })
                .ToList()
        };
    }
}
