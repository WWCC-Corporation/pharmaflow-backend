using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class VStockFefo
{
    public Guid? Id { get; set; }

    public string? Nombre { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public int? StockActual { get; set; }

    public string? Estado { get; set; }
}
