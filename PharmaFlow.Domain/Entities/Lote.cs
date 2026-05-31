using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class Lote
{
    public Guid Id { get; set; }

    public Guid? IdProducto { get; set; }

    public Guid? IdCompra { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Alerta> Alerta { get; set; } = new List<Alerta>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();

    public virtual StockLote? StockLote { get; set; }
}
