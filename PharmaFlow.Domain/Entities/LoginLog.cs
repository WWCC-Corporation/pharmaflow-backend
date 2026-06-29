using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class LoginLog
{
    public Guid Id { get; set; }

    public string? Correo { get; set; }

    public string? Ip { get; set; }

    public bool? Exito { get; set; }

    public DateTime CreatedAt { get; set; }
}
