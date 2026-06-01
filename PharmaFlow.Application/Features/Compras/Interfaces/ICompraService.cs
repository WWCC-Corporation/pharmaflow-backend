using PharmaFlow.Application.Features.Compras.DTOs.Compras;

namespace PharmaFlow.Application.Features.Compras.Interfaces;

public interface ICompraService
{
    Task<List<CompraResponseDto>> GetAllAsync();
    Task<CompraResponseDto?> GetByIdAsync(Guid id);
    Task<CompraResponseDto> CreateAsync(CreateCompraDto dto);
}