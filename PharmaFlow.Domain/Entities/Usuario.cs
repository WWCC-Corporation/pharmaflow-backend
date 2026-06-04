using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class Usuario
{
    public Guid Id { get; set; }

    public string Correo { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public int? IdRol { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Role? IdRolNavigation { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<TurnosCaja> TurnosCajas { get; set; } = new List<TurnosCaja>();

    public virtual ICollection<UsuarioSucursale> UsuarioSucursales { get; set; } = new List<UsuarioSucursale>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
