using PharmaFlow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class MovimientosCaja
{
    public Guid Id { get; set; }

    public TipoMovimientoCaja Tipo { get; set; }

    public Guid? IdTurnoCaja { get; set; }

    public Guid? IdVenta { get; set; }

    public Guid? IdUsuario { get; set; }

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual TurnosCaja? IdTurnoCajaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}


