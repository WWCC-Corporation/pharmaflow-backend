using PharmaFlow.Application.Caja.Commands;

namespace PharmaFlow.Application.Caja.Validators;

public static class CerrarCajaValidator
{
    public static void Validate(CerrarCajaCommand command)
    {
        if (command.IdTurnoCaja == Guid.Empty)
            throw new ArgumentException("El ID del turno de caja es obligatorio.");

        if (command.IdUsuario == Guid.Empty)
            throw new ArgumentException("El ID del usuario es obligatorio.");

        if (command.MontoContado < 0)
            throw new ArgumentException("El monto contado no puede ser negativo.");
    }
}
