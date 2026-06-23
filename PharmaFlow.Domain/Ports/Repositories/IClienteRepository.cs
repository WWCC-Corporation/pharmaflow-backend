using PharmaFlow.Application.Features.Clientes.DTOs;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public interface IClienteRepository
{
    Task<List<ClienteResponseDto>> ListarAsync(CancellationToken cancellationToken);

    Task<ClienteResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ClienteResponseDto> CrearAsync(CreateClienteDto dto, CancellationToken cancellationToken);

    Task<ClienteResponseDto?> ActualizarAsync(Guid id, UpdateClienteDto dto, CancellationToken cancellationToken);

    Task<bool> DesactivarAsync(Guid id, CancellationToken cancellationToken);
}
