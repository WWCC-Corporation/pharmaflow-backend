using MediatR;

namespace PharmaFlow.Application.Features.Compras.Commands.Proveedores;

public record DesactivarProveedorCommand(Guid Id) : IRequest<bool>;
