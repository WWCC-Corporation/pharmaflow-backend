using PharmaFlow.Application.Ventas.Commands;
using PharmaFlow.Application.Ventas.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Ventas.Handlers;

public class CrearVentaHandler
{
    private readonly IVentaRepository ventaRepository;

    public CrearVentaHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public async Task<VentaResponseDto> Handle(CrearVentaCommand command, CancellationToken cancellationToken)
    {
        var venta = new Venta
        {
            IdSucursal = command.Datos.IdSucursal,
            Moneda = command.Datos.Moneda,
            Metodo = command.Datos.Metodo,
            IdCliente = command.Datos.IdCliente,
            IdUsuario = command.Datos.IdUsuario,
            IdTurnoCaja = command.Datos.IdTurnoCaja,
            TipoCambio = command.Datos.TipoCambio,
            MontoTotal = command.Datos.MontoTotal,
            MontoRecibido = command.Datos.MontoRecibido,
            Vuelto = command.Datos.Vuelto,
            DetalleVenta = command.Datos.Detalles.Select(d => new DetalleVenta
            {
                IdLote = d.IdLote,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        };

        var resultado = await ventaRepository.CrearAsync(venta, cancellationToken);

        return new VentaResponseDto
        {
            Id = resultado.Id,
            IdSucursal = resultado.IdSucursal,
            Estado = resultado.Estado,
            Moneda = resultado.Moneda,
            Metodo = resultado.Metodo,
            IdCliente = resultado.IdCliente,
            IdUsuario = resultado.IdUsuario,
            IdTurnoCaja = resultado.IdTurnoCaja,
            Fecha = resultado.Fecha,
            TipoCambio = resultado.TipoCambio,
            MontoTotal = resultado.MontoTotal,
            MontoRecibido = resultado.MontoRecibido,
            Vuelto = resultado.Vuelto,
            Detalles = resultado.DetalleVenta.Select(d => new DetalleVentaResponseDto
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
