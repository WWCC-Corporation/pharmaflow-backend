using MediatR;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;

namespace PharmaFlow.Application.Features.Compras.Commands.Compras;

public record CrearCompraCommand(CreateCompraDto Datos) : IRequest<CompraResponseDto>;
