using System;
using System.Collections.Generic;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Domain.Entities;

public partial class Venta
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid? IdCliente { get; set; }

    public Guid? IdUsuario { get; set; }

    public Guid? IdTurnoCaja { get; set; }

    public DateTime Fecha { get; set; }

    public MetodoPago? Metodo { get; set; }

    public EstadoVenta Estado { get; set; }

    public Moneda Moneda { get; set; }

    public decimal TipoCambio { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal MontoRecibido { get; set; }

    public decimal Vuelto { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual TurnosCaja? IdTurnoCajaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();
}
