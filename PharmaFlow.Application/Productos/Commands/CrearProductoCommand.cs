namespace PharmaFlow.Application.Productos.Commands;

public class CrearProductoCommand
{
    public string Nombre { get; set; } = string.Empty;

    public string? PrincipioActivo { get; set; }

    public string? Laboratorio { get; set; }

    public string? FormaFarmaceutica { get; set; }

    public string? Concentracion { get; set; }

    public string? CodigoBarra { get; set; }

    public int StockMinimo { get; set; } = 5;

    public bool RequiereReceta { get; set; }
}
