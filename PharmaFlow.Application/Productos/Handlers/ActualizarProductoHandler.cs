using PharmaFlow.Application.Productos.Commands;
using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class ActualizarProductoHandler
{
    private readonly IProductoRepository productoRepository;

    public ActualizarProductoHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<ProductoDto?> Handle(ActualizarProductoCommand command, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (producto is null)
        {
            return null;
        }

        if (command.Nombre is not null)
        {
            if (string.IsNullOrWhiteSpace(command.Nombre))
            {
                throw new InvalidOperationException("El nombre del producto no puede estar vacio.");
            }

            producto.Nombre = command.Nombre.Trim();
        }

        if (command.StockMinimo.HasValue)
        {
            if (command.StockMinimo.Value < 0)
            {
                throw new InvalidOperationException("El stock minimo no puede ser negativo.");
            }

            producto.StockMinimo = command.StockMinimo.Value;
        }

        if (command.CodigoBarra is not null)
        {
            var codigoBarra = NormalizarTexto(command.CodigoBarra);
            if (!string.IsNullOrWhiteSpace(codigoBarra) &&
                await productoRepository.ExisteCodigoBarraAsync(codigoBarra, producto.Id, cancellationToken))
            {
                throw new InvalidOperationException("Ya existe un producto con ese codigo de barras.");
            }

            producto.CodigoBarra = codigoBarra;
        }

        if (command.PrincipioActivo is not null) producto.PrincipioActivo = NormalizarTexto(command.PrincipioActivo);
        if (command.Laboratorio is not null) producto.Laboratorio = NormalizarTexto(command.Laboratorio);
        if (command.FormaFarmaceutica is not null) producto.FormaFarmaceutica = NormalizarTexto(command.FormaFarmaceutica);
        if (command.Concentracion is not null) producto.Concentracion = NormalizarTexto(command.Concentracion);
        if (command.RequiereReceta.HasValue) producto.RequiereReceta = command.RequiereReceta.Value;
        producto.UpdatedAt = DateTime.UtcNow;

        await productoRepository.ActualizarAsync(producto, cancellationToken);

        return ProductoMapper.ToDto(producto);
    }

    private static string? NormalizarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
