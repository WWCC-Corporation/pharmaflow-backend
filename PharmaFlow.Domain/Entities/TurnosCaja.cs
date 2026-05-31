using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class TurnosCaja
{
    public Guid Id { get; set; }

    public Guid? IdUsuario { get; set; }

    public decimal? MontoApertura { get; set; }

    public decimal? MontoVentas { get; set; }

    public decimal? MontoContado { get; set; }

    public decimal? DiferenciaCaja { get; set; }

    public bool? Abierto { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
