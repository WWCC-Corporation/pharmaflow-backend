using PharmaFlow.Application.Sucursales.DTOs;
using PharmaFlow.Application.Sucursales.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Sucursales.Handlers;

public class ObtenerSucursalPorIdHandler
{
    private readonly ISucursalRepository sucursalRepository;

    public ObtenerSucursalPorIdHandler(ISucursalRepository sucursalRepository)
    {
        this.sucursalRepository = sucursalRepository;
    }

    public async Task<SucursalDto?> Handle(ObtenerSucursalPorIdQuery query, CancellationToken cancellationToken)
    {
        var sucursal = await sucursalRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        return sucursal is null ? null : SucursalMapper.ToDto(sucursal);
    }
}
