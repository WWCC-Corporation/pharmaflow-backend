namespace PharmaFlow.Application.Dashboard.DTOs;

public class ActividadRecienteDto
{
    public Guid Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal? Monto { get; set; }

    public DateTime Fecha { get; set; }
}
