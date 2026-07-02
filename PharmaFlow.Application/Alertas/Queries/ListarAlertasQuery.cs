using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Alertas.Queries;

public class ListarAlertasQuery
{
    public Guid? IdSucursal { get; set; }

    public bool? SoloNoLeidas { get; set; }

    public TipoAlerta? Tipo { get; set; }
}
