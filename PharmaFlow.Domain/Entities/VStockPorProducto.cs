using System;
using System.Collections.Generic;

namespace PharmaFlow.Domain.Entities;

public partial class VStockPorProducto
{
    public Guid? IdSucursal { get; set; }

    public string? Sucursal { get; set; }

    public Guid? Id { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoBarra { get; set; }

    public int? StockMinimo { get; set; }

    public long? StockTotal { get; set; }

    public string? Estado { get; set; }
}
