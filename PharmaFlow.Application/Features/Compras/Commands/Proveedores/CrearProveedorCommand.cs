using MediatR;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Commands.Proveedores;

public record CrearProveedorCommand(CreateProveedorDto Datos) : IRequest<ProveedorResponseDto>;
