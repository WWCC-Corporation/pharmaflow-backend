using System;
namespace PharmaFlow.Application.Inventario.DTOs;

public class AjustarStockRequestDto
{
    public Guid IdSucursal { get; set; }
    public Guid IdLote { get; set; }
    public Guid UsuarioId { get; set; }
    public int NuevaCantidadFisica { get; set; } 
}