using System.Net;
using System.Text.Json;

namespace PharmaFlow.Persistence.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pasa la petición al siguiente middleware o controlador
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si cualquier parte del código falla, lo atrapamos aquí
            _logger.LogError(ex, "Ocurrió una excepción no manejada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Aquí podrías validar si es una DomainException para devolver 400 en vez de 500
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Ha ocurrido un error interno en el servidor.",
            Detailed = exception.Message // En producción, es mejor ocultar este detalle
        });

        return context.Response.WriteAsync(result);
    }
}
