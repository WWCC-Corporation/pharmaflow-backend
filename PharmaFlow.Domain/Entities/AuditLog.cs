using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class AuditLog
{
    public Guid Id { get; set; }

    public Guid? UsuarioId { get; set; }

    public string? Accion { get; set; }

    public string? Tabla { get; set; }

    public Guid? RegistroId { get; set; }

    public string? Detalle { get; set; }

    public DateTime? CreatedAt { get; set; }
}
