using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class Proveedore
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Ruc { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
