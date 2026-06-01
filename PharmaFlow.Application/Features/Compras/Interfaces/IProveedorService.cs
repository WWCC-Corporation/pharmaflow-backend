using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Interfaces;

public interface IProveedorService
{
    Task<List<ProveedorResponseDto>> GetAllAsync();
    Task<ProveedorResponseDto?> GetByIdAsync(Guid id);
    Task<ProveedorResponseDto> CreateAsync(CreateProveedorDto dto);
    Task<ProveedorResponseDto?> UpdateAsync(Guid id, UpdateProveedorDto dto);
    Task<bool> DeleteAsync(Guid id);
}