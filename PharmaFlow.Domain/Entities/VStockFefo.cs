using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class VStockFefo
{
    public Guid? IdSucursal { get; set; }

    public string? Sucursal { get; set; }

    public Guid? Id { get; set; }

    public string? Nombre { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public int? StockActual { get; set; }

    public string? Estado { get; set; }
}
