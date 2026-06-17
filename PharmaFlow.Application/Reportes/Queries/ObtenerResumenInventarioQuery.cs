namespace PharmaFlow.Application.Reportes.Queries;

public class ObtenerResumenInventarioQuery
{
    public Guid? IdSucursal { get; set; }

    public int DiasVencimiento { get; set; } = 30;
}
