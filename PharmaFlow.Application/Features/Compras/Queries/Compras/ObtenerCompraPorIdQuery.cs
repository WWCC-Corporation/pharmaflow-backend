using MediatR;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;

namespace PharmaFlow.Application.Features.Compras.Queries.Compras;

public record ObtenerCompraPorIdQuery(Guid Id) : IRequest<CompraResponseDto?>;
