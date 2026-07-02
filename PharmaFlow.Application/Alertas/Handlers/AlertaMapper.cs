using PharmaFlow.Application.Alertas.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Alertas.Handlers;

public static class AlertaMapper
{
    public static AlertaDto ToDto(Alerta alerta)
    {
        return new AlertaDto
        {
            Id = alerta.Id,
            IdSucursal = alerta.IdSucursal,
            IdProducto = alerta.IdProducto,
            IdLote = alerta.IdLote,
            Tipo = alerta.Tipo,
            Mensaje = alerta.Mensaje,
            Leida = alerta.Leida,
            CreatedAt = alerta.CreatedAt
        };
    }
}
