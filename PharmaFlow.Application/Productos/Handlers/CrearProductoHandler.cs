using PharmaFlow.Application.Productos.Commands;
using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class CrearProductoHandler
{
    private readonly IProductoRepository productoRepository;

    public CrearProductoHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<ProductoDto> Handle(CrearProductoCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Nombre))
        {
            throw new InvalidOperationException("El nombre del producto es requerido.");
        }

        if (command.StockMinimo < 0)
        {
            throw new InvalidOperationException("El stock minimo no puede ser negativo.");
        }

        var codigoBarra = NormalizarTexto(command.CodigoBarra);
        if (!string.IsNullOrWhiteSpace(codigoBarra) &&
            await productoRepository.ExisteCodigoBarraAsync(codigoBarra, null, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe un producto con ese codigo de barras.");
        }

        var producto = new Producto
        {
            Nombre = command.Nombre.Trim(),
            PrincipioActivo = NormalizarTexto(command.PrincipioActivo),
            Laboratorio = NormalizarTexto(command.Laboratorio),
            FormaFarmaceutica = NormalizarTexto(command.FormaFarmaceutica),
            Concentracion = NormalizarTexto(command.Concentracion),
            CodigoBarra = codigoBarra,
            StockMinimo = command.StockMinimo,
            RequiereReceta = command.RequiereReceta,
            Activo = true
        };

        await productoRepository.AgregarAsync(producto, cancellationToken);

        return ProductoMapper.ToDto(producto);
    }

    private static string? NormalizarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
