using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Productos.Handlers;

public static class ProductoMapper
{
    public static ProductoDto ToDto(Producto producto)
    {
        return new ProductoDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            PrincipioActivo = producto.PrincipioActivo,
            Laboratorio = producto.Laboratorio,
            FormaFarmaceutica = producto.FormaFarmaceutica,
            Concentracion = producto.Concentracion,
            CodigoBarra = producto.CodigoBarra,
            StockMinimo = producto.StockMinimo,
            RequiereReceta = producto.RequiereReceta,
            Activo = producto.Activo
        };
    }
}
