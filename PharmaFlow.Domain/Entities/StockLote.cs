using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class StockLote
{
    public Guid Id { get; set; }

    public Guid? IdLote { get; set; }

    public int StockActual { get; set; }

    public int Version { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Lote? IdLoteNavigation { get; set; }
}
