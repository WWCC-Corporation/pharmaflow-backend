using Microsoft.AspNetCore.Mvc;

namespace PharmaFlow.Persistence.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    [HttpGet]
    public IActionResult Listar()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult Obtener(Guid id)
    {
        return Ok();
    }
}