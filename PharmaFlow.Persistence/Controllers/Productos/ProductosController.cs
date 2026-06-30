using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Productos.Commands;
using PharmaFlow.Application.Productos.Handlers;
using PharmaFlow.Application.Productos.Queries;

namespace PharmaFlow.Persistence.Controllers.Productos;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly ListarProductosHandler listarProductosHandler;
    private readonly ObtenerProductoPorIdHandler obtenerProductoPorIdHandler;
    private readonly BuscarProductoPorCodigoHandler buscarProductoPorCodigoHandler;
    private readonly CrearProductoHandler crearProductoHandler;
    private readonly ActualizarProductoHandler actualizarProductoHandler;
    private readonly DesactivarProductoHandler desactivarProductoHandler;

    public ProductosController(
        ListarProductosHandler listarProductosHandler,
        ObtenerProductoPorIdHandler obtenerProductoPorIdHandler,
        BuscarProductoPorCodigoHandler buscarProductoPorCodigoHandler,
        CrearProductoHandler crearProductoHandler,
        ActualizarProductoHandler actualizarProductoHandler,
        DesactivarProductoHandler desactivarProductoHandler)
    {
        this.listarProductosHandler = listarProductosHandler;
        this.obtenerProductoPorIdHandler = obtenerProductoPorIdHandler;
        this.buscarProductoPorCodigoHandler = buscarProductoPorCodigoHandler;
        this.crearProductoHandler = crearProductoHandler;
        this.actualizarProductoHandler = actualizarProductoHandler;
        this.desactivarProductoHandler = desactivarProductoHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarProductosQuery query, CancellationToken cancellationToken)
    {
        var productos = await listarProductosHandler.Handle(query, cancellationToken);

        return Ok(productos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var producto = await obtenerProductoPorIdHandler.Handle(new ObtenerProductoPorIdQuery { Id = id }, cancellationToken);

        if (producto is null)
        {
            return NotFound();
        }

        return Ok(producto);
    }

    [HttpGet("codigo/{codigoBarra}")]
    public async Task<IActionResult> BuscarPorCodigo(string codigoBarra, CancellationToken cancellationToken)
    {
        try
        {
            var producto = await buscarProductoPorCodigoHandler.Handle(new BuscarProductoPorCodigoQuery { CodigoBarra = codigoBarra }, cancellationToken);

            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearProductoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var producto = await crearProductoHandler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarProductoCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;

        try
        {
            var producto = await actualizarProductoHandler.Handle(command, cancellationToken);

            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancellationToken)
    {
        var desactivado = await desactivarProductoHandler.Handle(new DesactivarProductoCommand { Id = id }, cancellationToken);

        if (!desactivado)
        {
            return NotFound();
        }

        return NoContent();
    }
}
