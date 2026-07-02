namespace PharmaFlow.Application.Caja.Commands;

public class AbrirCajaCommand
{
    public Guid IdSucursal { get; set; }

    public Guid IdUsuario { get; set; }

    public decimal MontoApertura { get; set; }
}
