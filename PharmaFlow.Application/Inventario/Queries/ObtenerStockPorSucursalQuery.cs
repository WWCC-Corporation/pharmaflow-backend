using System;
namespace PharmaFlow.Application.Inventario.Queries;

public class ObtenerStockPorSucursalQuery
{
    public Guid IdSucursal { get; set; }

    public ObtenerStockPorSucursalQuery(Guid idSucursal)
    {
        IdSucursal = idSucursal;
    }
}