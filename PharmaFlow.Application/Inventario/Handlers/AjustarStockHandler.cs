using System;
using System.Threading;
using System.Threading.Tasks;
using PharmaFlow.Application.Inventario.Commands;
using PharmaFlow.Application.Inventario.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;
namespace PharmaFlow.Application.Inventario.Handlers;

public class AjustarStockHandler
{
    private readonly IStockLoteRepository _stockLoteRepository;
    private readonly IMovimientoInventarioRepository _movimientoRepository;

    public AjustarStockHandler(
        IStockLoteRepository stockLoteRepository, 
        IMovimientoInventarioRepository movimientoRepository)
    {
        _stockLoteRepository = stockLoteRepository;
        _movimientoRepository = movimientoRepository;
    }

    public async Task<MovimientoInventarioDto> EjecutarAsync(AjustarStockCommand command, CancellationToken cancellationToken = default)
    {
        var datos = command.Datos;
        
        if (datos.IdSucursal == Guid.Empty) throw new ArgumentException("La sucursal es requerida.");
        if (datos.IdLote == Guid.Empty) throw new ArgumentException("El lote es requerido.");
        if (datos.UsuarioId == Guid.Empty) throw new ArgumentException("El usuario responsable es requerido.");
        if (datos.NuevaCantidadFisica < 0) throw new ArgumentException("La cantidad física no puede ser negativa.");
        
        var stockActual = await _stockLoteRepository.ObtenerPorSucursalYLoteAsync(
            datos.IdSucursal, 
            datos.IdLote, 
            cancellationToken);
        
        if (stockActual == null)
        {
            throw new Exception("No se encontró registro de stock para este lote en la sucursal indicada.");
        }

        int diferencia = datos.NuevaCantidadFisica - stockActual.StockActual;
        
        stockActual.StockActual = datos.NuevaCantidadFisica;
        stockActual.UpdatedAt = DateTime.UtcNow;
        
        await _stockLoteRepository.ActualizarAsync(stockActual, cancellationToken);
        
        var movimiento = new MovimientoInventario
        {
            Id = Guid.NewGuid(),
            IdSucursal = datos.IdSucursal,
            IdLote = datos.IdLote,
            Tipo = TipoMovimiento.AJUSTE,
            Cantidad = diferencia, 
            UsuarioId = datos.UsuarioId,
            CreatedAt = DateTime.UtcNow
        };

        await _movimientoRepository.AgregarAsync(movimiento, cancellationToken);
        
        return new MovimientoInventarioDto
        {
            Id = movimiento.Id,
            IdSucursal = movimiento.IdSucursal,
            Tipo = movimiento.Tipo,
            IdProducto = movimiento.IdProducto,
            IdLote = movimiento.IdLote,
            Cantidad = movimiento.Cantidad,
            CreatedAt = movimiento.CreatedAt
        };
    }
}