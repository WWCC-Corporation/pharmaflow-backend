using PharmaFlow.Application.Features.Ventas.DTOs;
using PharmaFlow.Application.Features.Ventas.Queries;

namespace PharmaFlow.Application.Features.Ventas.Handlers;

public class ListarVentasHandler
{
    private readonly IVentaRepository ventaRepository;

    public ListarVentasHandler(IVentaRepository ventaRepository)
    {
        this.ventaRepository = ventaRepository;
    }

    public Task<List<VentaResponseDto>> Handle(ListarVentasQuery query, CancellationToken cancellationToken)
    {
        return ventaRepository.ListarAsync(cancellationToken);
    }
}
