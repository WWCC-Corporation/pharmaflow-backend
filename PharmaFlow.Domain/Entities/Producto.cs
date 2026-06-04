using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class Producto
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? PrincipioActivo { get; set; }

    public string? Laboratorio { get; set; }

    public string? FormaFarmaceutica { get; set; }

    public string? Concentracion { get; set; }

    public string? CodigoBarra { get; set; }

    public int StockMinimo { get; set; }

    public bool RequiereReceta { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Alerta> Alerta { get; set; } = new List<Alerta>();

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<Precio> Precios { get; set; } = new List<Precio>();
}
