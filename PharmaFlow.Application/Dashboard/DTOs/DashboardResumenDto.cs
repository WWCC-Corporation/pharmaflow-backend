namespace PharmaFlow.Application.Dashboard.DTOs;

public class DashboardResumenDto
{
    public DateTime FechaConsulta { get; set; }

    public Guid? IdSucursal { get; set; }

    public int VentasHoy { get; set; }

    public decimal MontoVendidoHoy { get; set; }

    public int ComprasHoy { get; set; }

    public int AlertasNoLeidas { get; set; }

    public int ProductosStockBajo { get; set; }

    public int ProductosPorVencer { get; set; }

    public IReadOnlyList<ActividadRecienteDto> VentasRecientes { get; set; } = [];

    public IReadOnlyList<ActividadRecienteDto> ComprasRecientes { get; set; } = [];

    public IReadOnlyList<ActividadRecienteDto> MovimientosCajaRecientes { get; set; } = [];
}
