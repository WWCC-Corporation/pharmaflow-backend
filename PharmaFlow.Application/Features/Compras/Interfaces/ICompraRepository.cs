using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Interfaces;

public interface ICompraRepository
{
    Task<List<Compra>> GetAllAsync();
    Task<Compra?> GetByIdAsync(Guid id);

    Task<Compra> CreateWithInventoryAsync(
        Compra compra,
        List<DetalleCompra> detalles,
        List<Lote> lotes,
        List<StockLote> stockLotes,
        List<MovimientoInventario> movimientos
    );

    Task<bool> ProveedorExistsAsync(Guid idProveedor);
    Task<bool> ProductoExistsAsync(Guid idProducto);
}