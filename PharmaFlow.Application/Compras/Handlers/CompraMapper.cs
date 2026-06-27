using PharmaFlow.Application.Compras.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Compras.Handlers;

public static class CompraMapper
{
    public static CompraDto ToDto(Compra compra)
    {
        return new CompraDto
        {
            Id = compra.Id,
            IdSucursal = compra.IdSucursal,
            IdProveedor = compra.IdProveedor,
            IdUsuario = compra.IdUsuario,
            Fecha = compra.Fecha,
            Estado = compra.Estado.ToString(),
            Moneda = compra.Moneda.ToString(),
            TipoCambio = compra.TipoCambio,
            Detalles = compra.DetalleCompras
                .Select(detalle => new DetalleCompraDto
                {
                    Id = detalle.Id,
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                })
                .ToList()
        };
    }
}
