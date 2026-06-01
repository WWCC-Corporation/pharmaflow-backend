using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Interfaces;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _proveedorRepository;

    public ProveedorService(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<List<ProveedorResponseDto>> GetAllAsync()
    {
        var proveedores = await _proveedorRepository.GetAllAsync();

        return proveedores.Select(MapToResponse).ToList();
    }

    public async Task<ProveedorResponseDto?> GetByIdAsync(Guid id)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(id);

        return proveedor is null ? null : MapToResponse(proveedor);
    }

    public async Task<ProveedorResponseDto> CreateAsync(CreateProveedorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        }

        var proveedor = new Proveedore
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre.Trim(),
            Ruc = dto.Ruc?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            Activo = true
        };

        var createdProveedor = await _proveedorRepository.CreateAsync(proveedor);

        return MapToResponse(createdProveedor);
    }

    public async Task<ProveedorResponseDto?> UpdateAsync(Guid id, UpdateProveedorDto dto)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(id);

        if (proveedor is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        }

        proveedor.Nombre = dto.Nombre.Trim();
        proveedor.Ruc = dto.Ruc?.Trim();
        proveedor.Telefono = dto.Telefono?.Trim();
        proveedor.Correo = dto.Correo?.Trim();
        proveedor.Activo = dto.Activo ?? proveedor.Activo;

        var updatedProveedor = await _proveedorRepository.UpdateAsync(proveedor);

        return updatedProveedor is null ? null : MapToResponse(updatedProveedor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _proveedorRepository.DeleteAsync(id);
    }

    private static ProveedorResponseDto MapToResponse(Proveedore proveedor)
    {
        return new ProveedorResponseDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Ruc = proveedor.Ruc,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Activo = proveedor.Activo
        };
    }
}