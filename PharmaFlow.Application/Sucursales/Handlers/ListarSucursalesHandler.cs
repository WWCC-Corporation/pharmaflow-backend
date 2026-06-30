using PharmaFlow.Application.Sucursales.DTOs;
using PharmaFlow.Application.Sucursales.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Sucursales.Handlers;

public class ListarSucursalesHandler
{
    private readonly ISucursalRepository sucursalRepository;

    public ListarSucursalesHandler(ISucursalRepository sucursalRepository)
    {
        this.sucursalRepository = sucursalRepository;
    }

    public async Task<IReadOnlyList<SucursalDto>> Handle(ListarSucursalesQuery query, CancellationToken cancellationToken)
    {
        var sucursales = await sucursalRepository.ListarAsync(query.SoloActivas, cancellationToken);

        return sucursales.Select(SucursalMapper.ToDto).ToList();
    }
}
