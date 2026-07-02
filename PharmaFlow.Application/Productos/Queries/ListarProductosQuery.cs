namespace PharmaFlow.Application.Productos.Queries;

public class ListarProductosQuery
{
    public string? Busqueda { get; set; }

    public bool? SoloActivos { get; set; }
}
