using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Ventas.DTOs;

public class VentaResponseDto
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public EstadoVenta? Estado { get; set; }

    public Moneda? Moneda { get; set; }

    public MetodoPago? Metodo { get; set; }

    public Guid? IdCliente { get; set; }

    public Guid? IdUsuario { get; set; }

    public Guid? IdTurnoCaja { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? TipoCambio { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal MontoRecibido { get; set; }

    public decimal Vuelto { get; set; }

    public List<DetalleVentaResponseDto> Detalles { get; set; } = new();
}
