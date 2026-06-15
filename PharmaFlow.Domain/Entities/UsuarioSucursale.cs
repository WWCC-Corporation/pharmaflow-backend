using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class UsuarioSucursale
{
    public Guid IdUsuario { get; set; }

    public Guid IdSucursal { get; set; }

    public bool Principal { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
