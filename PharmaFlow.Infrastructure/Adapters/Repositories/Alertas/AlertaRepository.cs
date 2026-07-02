using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Alertas;

public class AlertaRepository : IAlertaRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public AlertaRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Alerta>> ListarAsync(
        Guid? idSucursal,
        bool? soloNoLeidas,
        TipoAlerta? tipo,
        CancellationToken cancellationToken)
    {
        var alertas = dbContext.Alertas.AsQueryable();

        if (idSucursal.HasValue)
        {
            alertas = alertas.Where(alerta => alerta.IdSucursal == idSucursal.Value);
        }

        if (soloNoLeidas == true)
        {
            alertas = alertas.Where(alerta => !alerta.Leida);
        }

        if (tipo.HasValue)
        {
            alertas = alertas.Where(alerta => alerta.Tipo == tipo.Value);
        }

        return await alertas
            .AsNoTracking()
            .OrderBy(alerta => alerta.Leida)
            .ThenByDescending(alerta => alerta.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Alerta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Alertas.FirstOrDefaultAsync(alerta => alerta.Id == id, cancellationToken);
    }

    public async Task ActualizarAsync(Alerta alerta, CancellationToken cancellationToken)
    {
        dbContext.Alertas.Update(alerta);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
