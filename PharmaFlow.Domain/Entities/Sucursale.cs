using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class Sucursale
{
    public Guid Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Alerta> Alerta { get; set; } = new List<Alerta>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual ICollection<Precio> Precios { get; set; } = new List<Precio>();

    public virtual ICollection<StockLote> StockLotes { get; set; } = new List<StockLote>();

    public virtual ICollection<TurnosCaja> TurnosCajas { get; set; } = new List<TurnosCaja>();

    public virtual ICollection<UsuarioSucursale> UsuarioSucursales { get; set; } = new List<UsuarioSucursale>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
