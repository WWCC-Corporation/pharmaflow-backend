using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class DetalleCompra
{
    public Guid Id { get; set; }

    public Guid IdCompra { get; set; }

    public Guid IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
