namespace PharmaFlow.Application.Reportes.DTOs;

public class ResumenInventarioDto
{
    public Guid? IdSucursal { get; set; }

    public int ProductosStockBajo { get; set; }

    public int TotalProductosPorVencer { get; set; }

    public int ProductosVencidos { get; set; }

    public IReadOnlyList<ProductoStockDto> StockBajo { get; set; } = [];

    public IReadOnlyList<ProductoVencimientoDto> ProductosPorVencer { get; set; } = [];
}
