using PharmaFlow.Application.Compras.Commands;
using PharmaFlow.Application.Compras.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Compras.Handlers;

public class RegistrarCompraHandler
{
    private readonly ICompraRepository compraRepository;

    public RegistrarCompraHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public async Task<CompraDto> Handle(RegistrarCompraCommand command, CancellationToken cancellationToken)
    {
        if (command.IdSucursal == Guid.Empty)
        {
            throw new InvalidOperationException("La compra debe tener una sucursal.");
        }

        if (command.Detalles is null || command.Detalles.Count == 0)
        {
            throw new InvalidOperationException("La compra debe tener al menos un detalle de productos.");
        }

        foreach (var detalle in command.Detalles)
        {
            if (detalle.IdProducto == Guid.Empty)
            {
                throw new InvalidOperationException("Cada detalle debe tener un producto.");
            }

            if (detalle.Cantidad <= 0)
            {
                throw new InvalidOperationException("La cantidad de cada detalle debe ser mayor a cero.");
            }
        }

        var compra = new Compra
        {
            IdSucursal = command.IdSucursal,
            IdProveedor = command.IdProveedor,
            IdUsuario = command.IdUsuario,
            Moneda = command.Moneda,
            TipoCambio = command.TipoCambio <= 0 ? 1m : command.TipoCambio,
            Estado = EstadoCompra.pendiente,
            DetalleCompras = command.Detalles
                .Select(detalle => new DetalleCompra
                {
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                })
                .ToList()
        };

        await compraRepository.AgregarAsync(compra, cancellationToken);

        return CompraMapper.ToDto(compra);
    }
}
