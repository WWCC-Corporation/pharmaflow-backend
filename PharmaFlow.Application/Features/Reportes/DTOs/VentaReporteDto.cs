namespace PharmaFlow.Application.Features.Reportes.DTOs;

public class VentaReporteDto
{
    public DateTime Fecha { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
}
