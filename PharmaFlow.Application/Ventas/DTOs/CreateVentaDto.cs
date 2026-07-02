using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Ventas.DTOs;

public class CreateVentaDto
{
    public Guid IdSucursal { get; set; }

    public Moneda Moneda { get; set; }

    public MetodoPago? Metodo { get; set; }

    public Guid? IdCliente { get; set; }

    public Guid? IdUsuario { get; set; }

    public Guid? IdTurnoCaja { get; set; }

    public decimal TipoCambio { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal MontoRecibido { get; set; }

    public decimal Vuelto { get; set; }

    public List<CreateDetalleVentaDto> Detalles { get; set; } = new();
}
