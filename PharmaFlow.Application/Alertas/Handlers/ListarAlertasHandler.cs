using PharmaFlow.Application.Alertas.DTOs;
using PharmaFlow.Application.Alertas.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Alertas.Handlers;

public class ListarAlertasHandler
{
    private readonly IAlertaRepository alertaRepository;

    public ListarAlertasHandler(IAlertaRepository alertaRepository)
    {
        this.alertaRepository = alertaRepository;
    }

    public async Task<IReadOnlyList<AlertaDto>> Handle(ListarAlertasQuery query, CancellationToken cancellationToken)
    {
        var alertas = await alertaRepository.ListarAsync(
            query.IdSucursal,
            query.SoloNoLeidas,
            query.Tipo,
            cancellationToken);

        return alertas.Select(AlertaMapper.ToDto).ToList();
    }
}
