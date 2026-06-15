using PharmaFlow.Application.Features.Compras.DTOs;

namespace PharmaFlow.Application.Features.Compras.Handlers;

public interface ICompraRepository
{
    Task<List<CompraResponseDto>> ListarAsync(CancellationToken cancellationToken);

    Task<CompraResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<CompraResponseDto> CrearAsync(CreateCompraDto dto, CancellationToken cancellationToken);
}
