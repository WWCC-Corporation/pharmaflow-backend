namespace PharmaFlow.Application.Productos.Commands;

public class ActualizarProductoCommand
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string? PrincipioActivo { get; set; }

    public string? Laboratorio { get; set; }

    public string? FormaFarmaceutica { get; set; }

    public string? Concentracion { get; set; }

    public string? CodigoBarra { get; set; }

    public int? StockMinimo { get; set; }

    public bool? RequiereReceta { get; set; }
}
