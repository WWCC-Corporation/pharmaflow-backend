using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class Precio
{
    public Guid Id { get; set; }

    public Guid? IdSucursal { get; set; }

    public Guid IdProducto { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public DateTime VigenteDesde { get; set; }

    public bool Activo { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Sucursale? IdSucursalNavigation { get; set; }
}
