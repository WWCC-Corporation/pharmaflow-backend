using System;
using System.Collections.Generic;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Domain.Entities;

public partial class MovimientosCaja
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid? IdTurnoCaja { get; set; }

    public Guid? IdVenta { get; set; }

    public Guid? IdUsuario { get; set; }

    public TipoMovimientoCaja Tipo { get; set; }

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual TurnosCaja? IdTurnoCajaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
