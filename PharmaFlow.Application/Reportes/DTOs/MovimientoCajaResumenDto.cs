namespace PharmaFlow.Application.Reportes.DTOs;

public class MovimientoCajaResumenDto
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public string Tipo { get; set; } = null!;

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public DateTime Fecha { get; set; }
}
