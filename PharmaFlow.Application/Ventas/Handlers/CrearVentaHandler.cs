using PharmaFlow.Application.Features.Ventas.Commands;
using PharmaFlow.Application.Features.Ventas.DTOs;

namespace PharmaFlow.Application.Features.Ventas.Handlers;

public class CrearVentaHandler
{
    private readonly IVentaRepository ventaRepository;

    public CrearVentaHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public Task<VentaResponseDto> Handle(CrearVentaCommand command, CancellationToken cancellationToken)
    {
        return ventaRepository.CrearAsync(command.Datos, cancellationToken);
    }
}
