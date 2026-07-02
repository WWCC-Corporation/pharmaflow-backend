using PharmaFlow.Application.Compras.Commands;
using PharmaFlow.Application.Compras.DTOs;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Compras.Handlers;

public class RecepcionarCompraHandler
{
    private readonly ICompraRepository compraRepository;

    public RecepcionarCompraHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public async Task<CompraDto> Handle(RecepcionarCompraCommand command, CancellationToken cancellationToken)
    {
        if (command.IdCompra == Guid.Empty)
        {
            throw new InvalidOperationException("La compra es requerida.");
        }

        var compra = await compraRepository.ObtenerPorIdAsync(command.IdCompra, cancellationToken);

        if (compra is null)
        {
            throw new KeyNotFoundException("La compra no existe.");
        }

        if (compra.Estado == EstadoCompra.recepcionada)
        {
            throw new InvalidOperationException("La compra ya fue recepcionada.");
        }

        if (compra.Estado == EstadoCompra.anulada)
        {
            throw new InvalidOperationException("No se puede recepcionar una compra anulada.");
        }

        var detalles = command.Detalles.Select(detalle => new RecepcionCompraDetalle
        {
            IdDetalleCompra = detalle.IdDetalleCompra,
            NumeroLote = detalle.NumeroLote,
            FechaVencimiento = detalle.FechaVencimiento
        }).ToList();

        var recepcionada = await compraRepository.RecepcionarAsync(command.IdCompra, command.IdUsuario, detalles, cancellationToken);

        return CompraMapper.ToDto(recepcionada);
    }
}
