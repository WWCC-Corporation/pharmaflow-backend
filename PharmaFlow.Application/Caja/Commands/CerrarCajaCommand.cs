namespace PharmaFlow.Application.Caja.Commands;

public class CerrarCajaCommand
{
    public Guid IdTurnoCaja { get; set; }
    public Guid IdUsuario { get; set; }
    public decimal MontoContado { get; set; }
}
