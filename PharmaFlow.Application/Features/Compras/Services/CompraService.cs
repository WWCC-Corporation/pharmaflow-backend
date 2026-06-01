using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Interfaces;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _compraRepository;

    public CompraService(ICompraRepository compraRepository)
    {
        _compraRepository = compraRepository;
    }

    public async Task<List<CompraResponseDto>> GetAllAsync()
    {
        var compras = await _compraRepository.GetAllAsync();

        return compras.Select(MapToResponse).ToList();
    }

    public async Task<CompraResponseDto?> GetByIdAsync(Guid id)
    {
        var compra = await _compraRepository.GetByIdAsync(id);

        return compra is null ? null : MapToResponse(compra);
    }

    public async Task<CompraResponseDto> CreateAsync(CreateCompraDto dto)
    {
        if (dto.IdProveedor == Guid.Empty)
        {
            throw new ArgumentException("El proveedor es obligatorio.");
        }

        if (dto.Detalles.Count == 0)
        {
            throw new ArgumentException("La compra debe tener al menos un producto.");
        }

        var proveedorExiste = await _compraRepository.ProveedorExistsAsync(dto.IdProveedor);

        if (!proveedorExiste)
        {
            throw new KeyNotFoundException("El proveedor no existe.");
        }

        foreach (var detalle in dto.Detalles)
        {
            if (detalle.IdProducto == Guid.Empty)
            {
                throw new ArgumentException("El producto es obligatorio.");
            }

            if (detalle.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            if (detalle.PrecioUnitario <= 0)
            {
                throw new ArgumentException("El precio unitario debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(detalle.NumeroLote))
            {
                throw new ArgumentException("El número de lote es obligatorio.");
            }

            var productoExiste = await _compraRepository.ProductoExistsAsync(detalle.IdProducto);

            if (!productoExiste)
            {
                throw new KeyNotFoundException($"El producto con ID {detalle.IdProducto} no existe.");
            }
        }

        var compraId = Guid.NewGuid();

        var compra = new Compra
        {
            Id = compraId,
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

            var detalleCompra = new DetalleCompra
            {
                Id = Guid.NewGuid(),
                IdCompra = compraId,
                IdProducto = item.IdProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };

            var lote = new Lote
            {
                Id = loteId,
                IdProducto = item.IdProducto,
                IdCompra = compraId,
                NumeroLote = item.NumeroLote.Trim(),
                FechaVencimiento = item.FechaVencimiento,
                CreatedAt = DateTime.UtcNow
            };

            var stockLote = new StockLote
            {
                Id = Guid.NewGuid(),
                IdLote = loteId,
                StockActual = item.Cantidad,
                Version = 1,
                UpdatedAt = DateTime.UtcNow
            };

            var movimiento = new MovimientoInventario
            {
                Id = Guid.NewGuid(),
                Tipo = TipoMovimiento.ENTRADA,
                IdProducto = item.IdProducto,
                IdLote = loteId,
                IdCompra = compraId,
                Cantidad = item.Cantidad,
                UsuarioId = dto.IdUsuario,
                CreatedAt = DateTime.UtcNow
            };

            detalles.Add(detalleCompra);
            lotes.Add(lote);
            stockLotes.Add(stockLote);
            movimientos.Add(movimiento);
        }

        var createdCompra = await _compraRepository.CreateWithInventoryAsync(
            compra,
            detalles,
            lotes,
            stockLotes,
            movimientos
        );

        createdCompra.DetalleCompras = detalles;

        return MapToResponse(createdCompra);
    }

    private static CompraResponseDto MapToResponse(Compra compra)
    {
        return new CompraResponseDto
        {
            Id = compra.Id,
            IdProveedor = compra.IdProveedor,
            NombreProveedor = compra.IdProveedorNavigation?.Nombre,
            IdUsuario = compra.IdUsuario,
            Fecha = compra.Fecha,
            Estado = compra.Estado?.ToString(),
            Moneda = compra.Moneda?.ToString(),
            TipoCambio = compra.TipoCambio,
            Total = compra.DetalleCompras.Sum(d => d.Cantidad * (d.PrecioUnitario ?? 0)),
            Detalles = compra.DetalleCompras.Select(d => new DetalleCompraResponseDto
            {
                Id = d.Id,
                IdProducto = d.IdProducto,
                NombreProducto = d.IdProductoNavigation?.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Cantidad * (d.PrecioUnitario ?? 0)
            }).ToList()
        };
    }
}