namespace PharmaFlow.Application.Compras.Queries;

public class ListarComprasQuery
{
    public Guid? IdSucursal { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }
}
