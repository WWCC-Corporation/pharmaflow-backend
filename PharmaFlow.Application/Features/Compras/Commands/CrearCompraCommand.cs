using PharmaFlow.Application.Features.Compras.DTOs;

namespace PharmaFlow.Application.Features.Compras.Commands;

public class CrearCompraCommand
{
    public CreateCompraDto Datos { get; set; } = new();
}
