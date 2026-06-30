using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public UsuarioRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken cancellationToken)
    {
        return await UsuariosConRelaciones()
            .OrderBy(usuario => usuario.Nombres)
            .ThenBy(usuario => usuario.Apellidos)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await UsuariosConRelaciones()
            .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken)
    {
        return await UsuariosConRelaciones()
            .FirstOrDefaultAsync(usuario => usuario.Correo == correo, cancellationToken);
    }

    public async Task<bool> ExisteCorreoAsync(string correo, Guid? idExcluir, CancellationToken cancellationToken)
    {
        var usuarios = dbContext.Usuarios.Where(usuario => usuario.Correo == correo);

        if (idExcluir.HasValue)
        {
            usuarios = usuarios.Where(usuario => usuario.Id != idExcluir.Value);
        }

        return await usuarios.AnyAsync(cancellationToken);
    }

    public async Task AgregarAsync(Usuario usuario, IReadOnlyList<Guid> sucursales, CancellationToken cancellationToken)
    {
        usuario.Id = Guid.NewGuid();
        usuario.Activo = true;

        var index = 0;
        foreach (var idSucursal in sucursales.Distinct())
        {
            usuario.UsuarioSucursales.Add(new UsuarioSucursale
            {
                IdUsuario = usuario.Id,
                IdSucursal = idSucursal,
                Principal = index == 0,
                Activo = true
            });
            index++;
        }

        await dbContext.Usuarios.AddAsync(usuario, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Usuario usuario, IReadOnlyList<Guid>? sucursales, CancellationToken cancellationToken)
    {
        if (sucursales is not null)
        {
            var actuales = await dbContext.UsuarioSucursales
                .Where(relacion => relacion.IdUsuario == usuario.Id)
                .ToListAsync(cancellationToken);

            dbContext.UsuarioSucursales.RemoveRange(actuales);

            var index = 0;
            foreach (var idSucursal in sucursales.Distinct())
            {
                dbContext.UsuarioSucursales.Add(new UsuarioSucursale
                {
                    IdUsuario = usuario.Id,
                    IdSucursal = idSucursal,
                    Principal = index == 0,
                    Activo = true
                });
                index++;
            }
        }

        dbContext.Usuarios.Update(usuario);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Usuario> UsuariosConRelaciones()
    {
        return dbContext.Usuarios
            .FromSqlRaw("""
                SELECT
                    id,
                    correo::text AS correo,
                    password_hash,
                    nombres,
                    apellidos,
                    id_rol,
                    activo,
                    created_at,
                    updated_at
                FROM usuarios
                """)
            .Include(usuario => usuario.IdRolNavigation)
            .Include(usuario => usuario.UsuarioSucursales);
    }
}
