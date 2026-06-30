namespace PharmaFlow.Application.Compras.Commands;

public class RecepcionarCompraCommand
{
    public Guid IdCompra { get; set; }

    public Guid? IdUsuario { get; set; }

    public List<RecepcionarCompraDetalleInput> Detalles { get; set; } = new();
}

public class RecepcionarCompraDetalleInput
{
    public Guid IdDetalleCompra { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly? FechaVencimiento { get; set; }
}
