using PharmaFlow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class Compra
{
    public Guid Id { get; set; }

    public EstadoCompra? Estado { get; set; }

    public Moneda? Moneda { get; set; }

    public Guid? IdProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? TipoCambio { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedore? IdProveedorNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();
}


