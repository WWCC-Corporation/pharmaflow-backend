using PharmaFlow.Application.Inventario.DTOs;
namespace PharmaFlow.Application.Inventario.Commands;

public class AjustarStockCommand
{
    public AjustarStockRequestDto Datos { get; set; }

    public AjustarStockCommand(AjustarStockRequestDto datos)
    {
        Datos = datos;
    }
}