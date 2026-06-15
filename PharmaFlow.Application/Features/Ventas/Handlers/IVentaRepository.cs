using PharmaFlow.Application.Features.Ventas.DTOs;

namespace PharmaFlow.Application.Features.Ventas.Handlers;

public interface IVentaRepository
{
    Task<List<VentaResponseDto>> ListarAsync(CancellationToken cancellationToken);

    Task<VentaResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<VentaResponseDto> CrearAsync(CreateVentaDto dto, CancellationToken cancellationToken);

    Task<bool> AnularAsync(Guid id, CancellationToken cancellationToken);
}
