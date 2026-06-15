using PharmaFlow.Application.Features.Compras.Commands;
using PharmaFlow.Application.Features.Compras.DTOs;

namespace PharmaFlow.Application.Features.Compras.Handlers;

public class CrearCompraHandler
{
    private readonly ICompraRepository compraRepository;

    public CrearCompraHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public Task<CompraResponseDto> Handle(CrearCompraCommand command, CancellationToken cancellationToken)
    {
        var datos = command.Datos;

        // Reglas de negocio del flujo de Compras (validacion de entrada).
        if (datos.IdSucursal == Guid.Empty)
        {
            throw new ArgumentException("La sucursal es obligatoria.");
        }

        if (datos.IdProveedor == Guid.Empty)
        {
            throw new ArgumentException("El proveedor es obligatorio.");
        }

        if (datos.Detalles.Count == 0)
        {
            throw new ArgumentException("La compra debe tener al menos un producto.");
        }

        foreach (var detalle in datos.Detalles)
        {
            if (detalle.IdProducto == Guid.Empty)
            {
                throw new ArgumentException("El producto es obligatorio en cada detalle.");
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
                throw new ArgumentException("El numero de lote es obligatorio.");
            }
        }

        return compraRepository.CrearAsync(datos, cancellationToken);
    }
}
