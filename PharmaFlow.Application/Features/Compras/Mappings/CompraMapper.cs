using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Mappings;

/// <summary>
/// Conversiones entre la entidad Compra (y sus detalles) y sus DTOs de respuesta.
/// </summary>
public static class CompraMapper
{
    public static CompraResponseDto ToResponse(Compra compra)
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
