using PharmaFlow.Application.Features.Ventas.DTOs;
using PharmaFlow.Application.Features.Ventas.Queries;

namespace PharmaFlow.Application.Features.Ventas.Handlers;

public class ObtenerVentaPorIdHandler
{
    private readonly IVentaRepository ventaRepository;

    public ObtenerVentaPorIdHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public Task<VentaResponseDto?> Handle(ObtenerVentaPorIdQuery query, CancellationToken cancellationToken)
    {
        return ventaRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
    }
}
