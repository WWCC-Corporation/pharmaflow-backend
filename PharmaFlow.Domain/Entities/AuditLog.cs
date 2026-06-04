using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class AuditLog
{
    public Guid Id { get; set; }

    public Guid? UsuarioId { get; set; }

    public Guid? IdSucursal { get; set; }

    public string? Accion { get; set; }

    public string? Tabla { get; set; }

    public Guid? RegistroId { get; set; }

    public string? Detalle { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Sucursale? IdSucursalNavigation { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
