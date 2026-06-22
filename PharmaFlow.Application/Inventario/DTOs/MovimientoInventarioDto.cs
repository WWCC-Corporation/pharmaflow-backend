using System;
using PharmaFlow.Domain.Enums;
namespace PharmaFlow.Application.Inventario.DTOs;

public class MovimientoInventarioDto
{
    public Guid Id { get; set; }
    public Guid IdSucursal { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public Guid? IdProducto { get; set; }
    public Guid? IdLote { get; set; }
    public int Cantidad { get; set; }
    public DateTime CreatedAt { get; set; }
}