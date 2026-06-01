using PharmaFlow.Domain.Enums;
using System;

namespace PharmaFlow.Application.Features.Caja.DTOs
{
    public class AperturaCajaDto
    {
        public Guid IdUsuario { get; set; }
        public decimal MontoApertura { get; set; }
    }

    public class CierreCajaDto
    {
        public Guid IdTurnoCaja { get; set; }
        public decimal MontoContado { get; set; }
    }

    public class RegistrarMovimientoDto
    {
        public Guid IdTurnoCaja { get; set; }
        public decimal Monto { get; set; }
        public TipoMovimientoCaja Tipo { get; set; }
        public string? Descripcion { get; set; }
        public Guid? IdVenta { get; set; }
    }

    public class TurnoCajaResponseDto
    {
        public Guid Id { get; set; }
        public Guid? IdUsuario { get; set; }
        public decimal? MontoApertura { get; set; }
        public decimal? MontoVentas { get; set; }
        public decimal? MontoContado { get; set; }
        public decimal? DiferenciaCaja { get; set; }
        public bool? Abierto { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
