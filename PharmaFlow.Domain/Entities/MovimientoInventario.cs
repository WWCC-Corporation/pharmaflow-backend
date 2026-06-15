using System;
using System.Collections.Generic;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Domain.Entities;

public partial class MovimientoInventario
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public TipoMovimiento Tipo { get; set; }

    public Guid? IdProducto { get; set; }

    public Guid? IdLote { get; set; }

    public Guid? IdCompra { get; set; }

    public Guid? IdVenta { get; set; }

    public Guid? IdDetalleVenta { get; set; }

    public int Cantidad { get; set; }

    public Guid? UsuarioId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual DetalleVenta? IdDetalleVentaNavigation { get; set; }

    public virtual Lote? IdLoteNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual Venta? IdVentaNavigation { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
