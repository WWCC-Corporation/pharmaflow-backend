using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Caja.Commands;

public class RegistrarMovimientoCajaCommand
{
    public Guid IdTurnoCaja { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid IdUsuario { get; set; }

    public decimal Monto { get; set; }
    public TipoMovimientoCaja Tipo { get; set; }
    public string? Descripcion { get; set; }
    public Guid? IdVenta { get; set; }
}
