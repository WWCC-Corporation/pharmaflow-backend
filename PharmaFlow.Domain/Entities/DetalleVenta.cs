using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class DetalleVenta
{
    public Guid Id { get; set; }

    public Guid IdVenta { get; set; }

    public Guid? IdLote { get; set; }

    public Guid IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Lote? IdLoteNavigation { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();
}
