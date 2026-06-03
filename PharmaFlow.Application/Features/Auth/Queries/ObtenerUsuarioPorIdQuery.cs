namespace PharmaFlow.Application.Features.Auth.Queries;

public class ObtenerUsuarioPorIdQuery
    : IRequest<UsuarioDto>
{
    public Guid Id { get; set; }
}