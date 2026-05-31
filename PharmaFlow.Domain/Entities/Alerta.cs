using PharmaFlow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class Alerta
{
    public Guid Id { get; set; }

    public TipoAlerta? Tipo { get; set; }

    public Guid? IdProducto { get; set; }

    public Guid? IdLote { get; set; }

    public string? Mensaje { get; set; }

    public bool? Leida { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Lote? IdLoteNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}


