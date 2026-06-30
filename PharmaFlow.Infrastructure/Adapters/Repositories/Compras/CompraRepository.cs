using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Compras;

public class CompraRepository : ICompraRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public CompraRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AgregarAsync(Compra compra, CancellationToken cancellationToken)
    {
        await dbContext.Compras.AddAsync(compra, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Compra>> ListarAsync(Guid? idSucursal, DateTime? desde, DateTime? hasta, CancellationToken cancellationToken)
    {
        var compras = dbContext.Compras
            .AsNoTracking()
            .Include(compra => compra.DetalleCompras)
            .AsQueryable();

        if (idSucursal.HasValue)
        {
            compras = compras.Where(compra => compra.IdSucursal == idSucursal.Value);
        }

        if (desde.HasValue)
        {
            compras = compras.Where(compra => compra.Fecha >= desde.Value);
        }

        if (hasta.HasValue)
        {
            compras = compras.Where(compra => compra.Fecha <= hasta.Value);
        }

        return await compras
            .OrderByDescending(compra => compra.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task<Compra?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Compras
            .Include(compra => compra.DetalleCompras)
            .FirstOrDefaultAsync(compra => compra.Id == id, cancellationToken);
    }

    public async Task<Compra> RecepcionarAsync(
        Guid idCompra,
        Guid? idUsuario,
        IReadOnlyList<RecepcionCompraDetalle> detalles,
        CancellationToken cancellationToken)
    {
        var compra = await dbContext.Compras
            .Include(compra => compra.DetalleCompras)
            .FirstOrDefaultAsync(compra => compra.Id == idCompra, cancellationToken)
            ?? throw new KeyNotFoundException("La compra no existe.");

        if (compra.Estado != EstadoCompra.pendiente)
        {
            throw new InvalidOperationException("Solo se pueden recepcionar compras pendientes.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var detallesPorId = detalles.ToDictionary(detalle => detalle.IdDetalleCompra);

        foreach (var detalleCompra in compra.DetalleCompras)
        {
            detallesPorId.TryGetValue(detalleCompra.Id, out var detalleRecepcion);
            var numeroLote = string.IsNullOrWhiteSpace(detalleRecepcion?.NumeroLote)
                ? $"COMP-{compra.Id.ToString()[..8]}-{detalleCompra.Id.ToString()[..8]}"
                : detalleRecepcion.NumeroLote.Trim();
            var fechaVencimiento = detalleRecepcion?.FechaVencimiento ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2));

            var lote = await dbContext.Lotes
                .FirstOrDefaultAsync(l =>
                    l.IdSucursal == compra.IdSucursal &&
                    l.IdProducto == detalleCompra.IdProducto &&
                    l.NumeroLote == numeroLote,
                    cancellationToken);

            if (lote is null)
            {
                lote = new Lote
                {
                    Id = Guid.NewGuid(),
                    IdSucursal = compra.IdSucursal,
                    IdProducto = detalleCompra.IdProducto,
                    IdCompra = compra.Id,
                    NumeroLote = numeroLote,
                    FechaVencimiento = fechaVencimiento,
                    CreatedAt = DateTime.UtcNow
                };

                await dbContext.Lotes.AddAsync(lote, cancellationToken);
            }

            var stock = await dbContext.StockLotes
                .FirstOrDefaultAsync(s => s.IdSucursal == compra.IdSucursal && s.IdLote == lote.Id, cancellationToken);

            if (stock is null)
            {
                stock = new StockLote
                {
                    Id = Guid.NewGuid(),
                    IdSucursal = compra.IdSucursal,
                    IdLote = lote.Id,
                    StockActual = detalleCompra.Cantidad,
                    Version = 1,
                    UpdatedAt = DateTime.UtcNow
                };

                await dbContext.StockLotes.AddAsync(stock, cancellationToken);
            }
            else
            {
                stock.StockActual += detalleCompra.Cantidad;
                stock.Version++;
                stock.UpdatedAt = DateTime.UtcNow;
            }

            dbContext.MovimientoInventarios.Add(new MovimientoInventario
            {
                Id = Guid.NewGuid(),
                IdSucursal = compra.IdSucursal,
                Tipo = TipoMovimiento.ENTRADA,
                IdProducto = detalleCompra.IdProducto,
                IdLote = lote.Id,
                IdCompra = compra.Id,
                Cantidad = detalleCompra.Cantidad,
                UsuarioId = idUsuario ?? compra.IdUsuario,
                CreatedAt = DateTime.UtcNow
            });
        }

        compra.Estado = EstadoCompra.recepcionada;
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return compra;
    }
}
