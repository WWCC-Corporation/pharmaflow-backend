using PharmaFlow.Application.Caja.Commands;

namespace PharmaFlow.Application.Caja.Validators;

public static class AbrirCajaValidator
{
    public static void Validate(AbrirCajaCommand command)
    {
        if (command.IdSucursal == Guid.Empty)
            throw new ArgumentException("El ID de la sucursal es obligatorio.");

        if (command.IdUsuario == Guid.Empty)
            throw new ArgumentException("El ID del usuario es obligatorio.");

        if (command.MontoApertura < 0)
            throw new ArgumentException("El monto de apertura no puede ser negativo.");
    }
}
