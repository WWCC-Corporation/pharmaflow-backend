using PharmaFlow.Application.Ventas.Commands;

namespace PharmaFlow.Application.Ventas.Handlers;

public class AnularVentaHandler
{
    private readonly IVentaRepository ventaRepository;

    public AnularVentaHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public Task<bool> Handle(AnularVentaCommand command, CancellationToken cancellationToken)
    {
        return ventaRepository.AnularAsync(command.Id, cancellationToken);
    }
}
