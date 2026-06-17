namespace PharmaFlow.Application.Reportes.Queries;

public class ObtenerResumenCajaQuery
{
    public Guid? IdSucursal { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }
}
