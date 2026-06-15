using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Commands.Compras;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Handlers.Compras;

public class CrearCompraHandler : IRequestHandler<CrearCompraCommand, CompraResponseDto>
{
    private readonly ICompraRepository _compraRepository;

    public CrearCompraHandler(ICompraRepository compraRepository)
    {
        _compraRepository = compraRepository;
    }

    public async Task<CompraResponseDto> Handle(CrearCompraCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Datos;

        // Validaciones de formato se ejecutan en CrearCompraValidator (FluentValidation).
        // Aquí solo se verifican reglas de negocio que dependen de la base de datos.
        var proveedorExiste = await _compraRepository.ProveedorExistsAsync(dto.IdProveedor);

        if (!proveedorExiste)
        {
            throw new KeyNotFoundException("El proveedor no existe.");
        }

        foreach (var detalle in dto.Detalles)
        {
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
                IdProducto = item.IdProducto,
                IdCompra = compraId,
                NumeroLote = item.NumeroLote.Trim(),
                FechaVencimiento = item.FechaVencimiento,
                CreatedAt = DateTime.UtcNow
            });

            stockLotes.Add(new StockLote
            {
                Id = Guid.NewGuid(),
                IdLote = loteId,
                StockActual = item.Cantidad,
                Version = 1,
                UpdatedAt = DateTime.UtcNow
            });

            // La recepción de mercadería genera un movimiento de inventario de tipo ENTRADA.
            movimientos.Add(new MovimientoInventario
            {
                Id = Guid.NewGuid(),
                Tipo = TipoMovimiento.ENTRADA,
                IdProducto = item.IdProducto,
                IdLote = loteId,
                IdCompra = compraId,
                Cantidad = item.Cantidad,
                UsuarioId = dto.IdUsuario,
                CreatedAt = DateTime.UtcNow
            });
        }

        var creada = await _compraRepository.CreateWithInventoryAsync(
            compra,
            detalles,
            lotes,
            stockLotes,
            movimientos
        );

        creada.DetalleCompras = detalles;

        return CompraMapper.ToResponse(creada);
    }
}
