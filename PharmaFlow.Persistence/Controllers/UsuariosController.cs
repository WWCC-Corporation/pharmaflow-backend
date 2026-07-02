using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Usuarios.Commands;
using PharmaFlow.Application.Usuarios.Handlers;
using PharmaFlow.Application.Usuarios.Queries;

namespace PharmaFlow.Persistence.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly ListarUsuariosHandler listarUsuariosHandler;
    private readonly ObtenerUsuarioPorIdHandler obtenerUsuarioPorIdHandler;
    private readonly CrearUsuarioHandler crearUsuarioHandler;
    private readonly ActualizarUsuarioHandler actualizarUsuarioHandler;

    public UsuariosController(
        ListarUsuariosHandler listarUsuariosHandler,
        ObtenerUsuarioPorIdHandler obtenerUsuarioPorIdHandler,
        CrearUsuarioHandler crearUsuarioHandler,
        ActualizarUsuarioHandler actualizarUsuarioHandler)
    {
        this.listarUsuariosHandler = listarUsuariosHandler;
        this.obtenerUsuarioPorIdHandler = obtenerUsuarioPorIdHandler;
        this.crearUsuarioHandler = crearUsuarioHandler;
        this.actualizarUsuarioHandler = actualizarUsuarioHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var usuarios = await listarUsuariosHandler.Handle(new ListarUsuariosQuery(), cancellationToken);

        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var usuario = await obtenerUsuarioPorIdHandler.Handle(new ObtenerUsuarioPorIdQuery { Id = id }, cancellationToken);

        if (usuario is null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await crearUsuarioHandler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(Obtener), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarUsuarioCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;

        try
        {
            var usuario = await actualizarUsuarioHandler.Handle(command, cancellationToken);

            if (usuario is null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }
}
