using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Features.Proveedores.DTOs;
using PharmaFlow.Application.Features.Proveedores.Handlers;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Infrastructure.Context;

namespace PharmaFlow.Infrastructure.Repositories.Proveedores;

public class ProveedorRepository : IProveedorRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public ProveedorRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ProveedorResponseDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var proveedores = await dbContext.Proveedores
            .AsNoTracking()
            .Where(proveedor => proveedor.Activo)
            .OrderBy(proveedor => proveedor.Nombre)
            .ToListAsync(cancellationToken);

        return proveedores.Select(MapToResponse).ToList();
    }

    public async Task<ProveedorResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var proveedor = await dbContext.Proveedores
            .AsNoTracking()
            .FirstOrDefaultAsync(proveedor => proveedor.Id == id, cancellationToken);

        return proveedor is null ? null : MapToResponse(proveedor);
    }

    public async Task<ProveedorResponseDto> CrearAsync(CreateProveedorDto dto, CancellationToken cancellationToken)
    {
        var proveedor = new Proveedore
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre.Trim(),
            Ruc = dto.Ruc?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            Activo = true
        };

        await dbContext.Proveedores.AddAsync(proveedor, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(proveedor);
    }

    public async Task<ProveedorResponseDto?> ActualizarAsync(Guid id, UpdateProveedorDto dto, CancellationToken cancellationToken)
    {
        var proveedor = await dbContext.Proveedores
            .FirstOrDefaultAsync(proveedor => proveedor.Id == id, cancellationToken);

        if (proveedor is null)
        {
            return null;
        }

        proveedor.Nombre = dto.Nombre.Trim();
        proveedor.Ruc = dto.Ruc?.Trim();
        proveedor.Telefono = dto.Telefono?.Trim();
        proveedor.Correo = dto.Correo?.Trim();
        proveedor.Activo = dto.Activo ?? proveedor.Activo;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(proveedor);
    }

    public async Task<bool> DesactivarAsync(Guid id, CancellationToken cancellationToken)
    {
        var proveedor = await dbContext.Proveedores
            .FirstOrDefaultAsync(proveedor => proveedor.Id == id, cancellationToken);

        if (proveedor is null)
        {
            return false;
        }

        // Baja logica: el proveedor no se elimina fisicamente, solo se desactiva.
        proveedor.Activo = false;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
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
