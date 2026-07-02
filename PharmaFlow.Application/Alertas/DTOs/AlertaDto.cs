using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Alertas.DTOs;

public class AlertaDto
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid? IdProducto { get; set; }

    public Guid? IdLote { get; set; }

    public TipoAlerta? Tipo { get; set; }

    public string? Mensaje { get; set; }

    public bool Leida { get; set; }

    public DateTime CreatedAt { get; set; }
}
