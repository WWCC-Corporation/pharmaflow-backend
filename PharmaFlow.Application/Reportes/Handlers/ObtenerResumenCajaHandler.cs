using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public class ObtenerResumenCajaHandler
{
    private readonly IReporteCajaReader reporteCajaReader;

    public ObtenerResumenCajaHandler(IReporteCajaReader reporteCajaReader)
    {
        this.reporteCajaReader = reporteCajaReader;
    }

    public Task<ResumenCajaDto> Handle(ObtenerResumenCajaQuery query, CancellationToken cancellationToken)
    {
        return reporteCajaReader.ObtenerResumenAsync(query, cancellationToken);
    }
}
