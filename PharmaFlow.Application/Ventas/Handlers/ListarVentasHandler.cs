using PharmaFlow.Application.Ventas.DTOs;
using PharmaFlow.Application.Ventas.Queries;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Ventas.Handlers;

public class ListarVentasHandler
{
    private readonly IVentaRepository ventaRepository;

    public ListarVentasHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public async Task<List<VentaResponseDto>> Handle(ListarVentasQuery query, CancellationToken cancellationToken)
    {
        var ventas = await ventaRepository.ListarAsync(cancellationToken);

        return ventas.Select(v => new VentaResponseDto
        {
            Id = v.Id,
            Estado = v.Estado,
            Moneda = v.Moneda,
            Metodo = v.Metodo,
            IdCliente = v.IdCliente,
            IdUsuario = v.IdUsuario,
            IdTurnoCaja = v.IdTurnoCaja,
            Fecha = v.Fecha,
            TipoCambio = v.TipoCambio,
            MontoTotal = v.MontoTotal,
            MontoRecibido = v.MontoRecibido,
            Vuelto = v.Vuelto,
            Detalles = v.DetalleVenta.Select(d => new DetalleVentaResponseDto
            {
                Id = d.Id,
                IdVenta = d.IdVenta,
                IdLote = d.IdLote,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        }).ToList();
    }
}
