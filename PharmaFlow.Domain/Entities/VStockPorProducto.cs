using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class VStockPorProducto
{
    public Guid? Id { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoBarra { get; set; }

    public int? StockMinimo { get; set; }

    public long? StockTotal { get; set; }

    public string? Estado { get; set; }
}
