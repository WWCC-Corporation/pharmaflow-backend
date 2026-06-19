namespace PharmaFlow.Application.Reportes.Queries;

public class ObtenerResumenVentasQuery
{
    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public Guid? IdSucursal { get; set; }
}
