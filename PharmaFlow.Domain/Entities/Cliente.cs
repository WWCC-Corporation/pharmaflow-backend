using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class Cliente
{
    public Guid Id { get; set; }

    public string? Dni { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
