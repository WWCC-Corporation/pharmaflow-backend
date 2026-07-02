using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Auth.Commands;
using PharmaFlow.Application.Auth.DTOs;
using PharmaFlow.Application.Auth.Handlers;

namespace PharmaFlow.Persistence.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler loginHandler;
    private readonly RefreshTokenHandler refreshTokenHandler;
    private readonly LogoutHandler logoutHandler;

    public AuthController(
        LoginHandler loginHandler,
        RefreshTokenHandler refreshTokenHandler,
        LogoutHandler logoutHandler)
    {
        this.loginHandler = loginHandler;
        this.refreshTokenHandler = refreshTokenHandler;
        this.logoutHandler = logoutHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var respuesta = await loginHandler.Handle(new LoginCommand { Request = request }, cancellationToken);

        if (respuesta is null)
        {
            return Unauthorized(new { mensaje = "Credenciales invalidas." });
        }

        return Ok(respuesta);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var respuesta = await refreshTokenHandler.Handle(command, cancellationToken);

        if (respuesta is null)
        {
            return Unauthorized(new { mensaje = "Refresh token invalido o expirado." });
        }

        return Ok(respuesta);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await logoutHandler.Handle(command, cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }
}
