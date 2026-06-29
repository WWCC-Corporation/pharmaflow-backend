using PharmaFlow.Application.Ventas.DTOs;
using PharmaFlow.Application.Ventas.Queries;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Ventas.Handlers;

public class ObtenerVentaPorIdHandler
{
    private readonly IVentaRepository ventaRepository;

    public ObtenerVentaPorIdHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public async Task<VentaResponseDto?> Handle(ObtenerVentaPorIdQuery query, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        if (venta is null)
        {
            return null;
        }

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
            Detalles = venta.DetalleVenta.Select(d => new DetalleVentaResponseDto
            {
                Id = d.Id,
                IdVenta = d.IdVenta,
                IdLote = d.IdLote,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        };
    }
}
