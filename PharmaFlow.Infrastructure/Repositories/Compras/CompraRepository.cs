using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Features.Compras.DTOs;
using PharmaFlow.Application.Features.Compras.Handlers;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Context;

namespace PharmaFlow.Infrastructure.Repositories.Compras;

public class CompraRepository : ICompraRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public CompraRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<CompraResponseDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var compras = await dbContext.Compras
            .AsNoTracking()
            .Include(compra => compra.IdProveedorNavigation)
            .Include(compra => compra.DetalleCompras)
                .ThenInclude(detalle => detalle.IdProductoNavigation)
            .OrderByDescending(compra => compra.Fecha)
            .ToListAsync(cancellationToken);

        return compras.Select(MapToResponse).ToList();
    }

    public async Task<CompraResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var compra = await dbContext.Compras
            .AsNoTracking()
            .Include(compra => compra.IdProveedorNavigation)
            .Include(compra => compra.DetalleCompras)
                .ThenInclude(detalle => detalle.IdProductoNavigation)
            .FirstOrDefaultAsync(compra => compra.Id == id, cancellationToken);

        return compra is null ? null : MapToResponse(compra);
    }

    public async Task<CompraResponseDto> CrearAsync(CreateCompraDto dto, CancellationToken cancellationToken)
    {
        // Reglas de negocio que dependen de la base de datos.
        var proveedorExiste = await dbContext.Proveedores
            .AnyAsync(proveedor => proveedor.Id == dto.IdProveedor && proveedor.Activo, cancellationToken);

        if (!proveedorExiste)
        {
            throw new KeyNotFoundException("El proveedor no existe o esta inactivo.");
        }

        foreach (var detalle in dto.Detalles)
        {
            var productoExiste = await dbContext.Productos
                .AnyAsync(producto => producto.Id == detalle.IdProducto, cancellationToken);

            if (!productoExiste)
            {
                throw new KeyNotFoundException($"El producto con ID {detalle.IdProducto} no existe.");
            }
        }

        var compraId = Guid.NewGuid();

        var compra = new Compra
        {
            Id = compraId,
            IdSucursal = dto.IdSucursal,
            IdProveedor = dto.IdProveedor,
            IdUsuario = dto.IdUsuario,
            Fecha = DateTime.UtcNow,
            Estado = EstadoCompra.recepcionada,
            Moneda = dto.Moneda,
            TipoCambio = dto.TipoCambio
        };

        var detalles = new List<DetalleCompra>();
        var lotes = new List<Lote>();
        var stockLotes = new List<StockLote>();
        var movimientos = new List<MovimientoInventario>();

        foreach (var item in dto.Detalles)
        {
            var loteId = Guid.NewGuid();

            detalles.Add(new DetalleCompra
            {
                Id = Guid.NewGuid(),
                IdCompra = compraId,
                IdProducto = item.IdProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            });

            lotes.Add(new Lote
            {
                Id = loteId,
                IdSucursal = dto.IdSucursal,
                IdProducto = item.IdProducto,
                IdCompra = compraId,
                NumeroLote = item.NumeroLote.Trim(),
                FechaVencimiento = item.FechaVencimiento,
                CreatedAt = DateTime.UtcNow
            });

            stockLotes.Add(new StockLote
            {
                Id = Guid.NewGuid(),
                IdSucursal = dto.IdSucursal,
                IdLote = loteId,
                StockActual = item.Cantidad,
                Version = 1,
                UpdatedAt = DateTime.UtcNow
            });

            // La recepcion de mercaderia genera un movimiento de inventario de tipo ENTRADA
            // que afecta el stock de la sucursal de la compra.
            movimientos.Add(new MovimientoInventario
            {
                Id = Guid.NewGuid(),
                IdSucursal = dto.IdSucursal,
                Tipo = TipoMovimiento.ENTRADA,
                IdProducto = item.IdProducto,
                IdLote = loteId,
                IdCompra = compraId,
                Cantidad = item.Cantidad,
                UsuarioId = dto.IdUsuario,
                CreatedAt = DateTime.UtcNow
            });
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await dbContext.Compras.AddAsync(compra, cancellationToken);
            await dbContext.DetalleCompras.AddRangeAsync(detalles, cancellationToken);
            await dbContext.Lotes.AddRangeAsync(lotes, cancellationToken);
            await dbContext.StockLotes.AddRangeAsync(stockLotes, cancellationToken);
            await dbContext.MovimientoInventarios.AddRangeAsync(movimientos, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        var compraCreada = await ObtenerPorIdAsync(compraId, cancellationToken);

        return compraCreada!;
    }

    private static CompraResponseDto MapToResponse(Compra compra)
    {
        return new CompraResponseDto
        {
            Id = compra.Id,
            IdSucursal = compra.IdSucursal,
            IdProveedor = compra.IdProveedor,
            NombreProveedor = compra.IdProveedorNavigation?.Nombre,
            IdUsuario = compra.IdUsuario,
            Fecha = compra.Fecha,
            Estado = compra.Estado.ToString(),
            Moneda = compra.Moneda.ToString(),
            TipoCambio = compra.TipoCambio,
            Total = compra.DetalleCompras.Sum(detalle => detalle.Cantidad * (detalle.PrecioUnitario ?? 0)),
            Detalles = compra.DetalleCompras.Select(detalle => new DetalleCompraResponseDto
            {
                Id = detalle.Id,
                IdProducto = detalle.IdProducto,
                NombreProducto = detalle.IdProductoNavigation?.Nombre,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Cantidad * (detalle.PrecioUnitario ?? 0)
            }).ToList()
        };
    }
}
