using MediatR;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Queries.Proveedores;

public record ObtenerProveedorPorIdQuery(Guid Id) : IRequest<ProveedorResponseDto?>;
