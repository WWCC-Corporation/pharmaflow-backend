using PharmaFlow.Application.Usuarios.Commands;

namespace PharmaFlow.Application.Usuarios.Validators;

public class UsuarioValidator
{
    public bool Validate(CrearUsuarioCommand command)
    {
        return !string.IsNullOrWhiteSpace(command.Correo);
    }
}