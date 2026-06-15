using FluentValidation;
using PharmaFlow.Application.Features.Compras.Commands.Compras;

namespace PharmaFlow.Application.Features.Compras.Validators.Compras;

public class CrearCompraValidator : AbstractValidator<CrearCompraCommand>
{
    public CrearCompraValidator()
    {
        RuleFor(x => x.Datos.IdProveedor)
            .NotEmpty().WithMessage("El proveedor es obligatorio.");

        RuleFor(x => x.Datos.Detalles)
            .NotEmpty().WithMessage("La compra debe tener al menos un producto.");

        RuleForEach(x => x.Datos.Detalles).ChildRules(detalle =>
        {
            detalle.RuleFor(d => d.IdProducto)
                .NotEmpty().WithMessage("El producto es obligatorio.");

            detalle.RuleFor(d => d.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

            detalle.RuleFor(d => d.PrecioUnitario)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");

            detalle.RuleFor(d => d.NumeroLote)
                .NotEmpty().WithMessage("El número de lote es obligatorio.")
                .MaximumLength(50).WithMessage("El número de lote no puede superar los 50 caracteres.");
        });
    }
}
