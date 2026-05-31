using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class DetalleVenta
{
    public Guid Id { get; set; }

    public Guid? IdVenta { get; set; }

    public Guid? IdLote { get; set; }

    public Guid? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public virtual Lote? IdLoteNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();
}
