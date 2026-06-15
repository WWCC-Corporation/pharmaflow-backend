using FluentValidation;
using PharmaFlow.Application.Features.Compras.Commands.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Validators.Proveedores;

public class CrearProveedorValidator : AbstractValidator<CrearProveedorCommand>
{
    public CrearProveedorValidator()
    {
        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre del proveedor es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

        RuleFor(x => x.Datos.Ruc)
            .MaximumLength(20).WithMessage("El RUC no puede superar los 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.Ruc));

        RuleFor(x => x.Datos.Telefono)
            .MaximumLength(20).WithMessage("El teléfono no puede superar los 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.Telefono));

        RuleFor(x => x.Datos.Correo)
            .EmailAddress().WithMessage("El correo no tiene un formato válido.")
            .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.Correo));
    }
}
