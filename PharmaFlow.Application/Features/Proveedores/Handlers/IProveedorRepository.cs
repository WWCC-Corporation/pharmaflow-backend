using PharmaFlow.Application.Features.Proveedores.DTOs;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public interface IProveedorRepository
{
    Task<List<ProveedorResponseDto>> ListarAsync(CancellationToken cancellationToken);

    Task<ProveedorResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ProveedorResponseDto> CrearAsync(CreateProveedorDto dto, CancellationToken cancellationToken);

    Task<ProveedorResponseDto?> ActualizarAsync(Guid id, UpdateProveedorDto dto, CancellationToken cancellationToken);

    Task<bool> DesactivarAsync(Guid id, CancellationToken cancellationToken);
}
