using PharmaFlow.Application.Alertas.Commands;
using PharmaFlow.Application.Alertas.DTOs;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Alertas.Handlers;

public class MarcarAlertaLeidaHandler
{
    private readonly IAlertaRepository alertaRepository;

    public MarcarAlertaLeidaHandler(IAlertaRepository alertaRepository)
    {
        this.alertaRepository = alertaRepository;
    }

    public async Task<AlertaDto?> Handle(MarcarAlertaLeidaCommand command, CancellationToken cancellationToken)
    {
        var alerta = await alertaRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (alerta is null)
        {
            return null;
        }

        alerta.Leida = true;
        await alertaRepository.ActualizarAsync(alerta, cancellationToken);

        return AlertaMapper.ToDto(alerta);
    }
}
