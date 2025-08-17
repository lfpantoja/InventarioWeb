using Microsoft.AspNetCore.Mvc;
using ProductosAplicacion.Abstracciones;
using ProductosAplicacion.Errores;
using ProductosDominio.Entidades;

[ApiController]
[Route("api/productos")]
public class ProductosController(IProductoServicio servicio) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? nombre, [FromQuery] string? categoria)
        => Ok(await servicio.ListarAsync(nombre, categoria));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obtener(Guid id)
        => (await servicio.ObtenerPorIdAsync(id)) is { } p ? Ok(p) : NotFound(new { mensaje = "No encontrado" });

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Producto p)
    {
        try
        {
            var creado = await servicio.CrearAsync(p);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }
        catch (NombreProductoDuplicadoException ex)
        {
            return Conflict(new { codigo = "NOMBRE_DUPLICADO", mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] Producto p)
        => await servicio.ActualizarAsync(id, p) ? Ok(new { mensaje = "Actualizado correctamente" }) : NotFound(new { mensaje = "No encontrado" });

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id)
        => await servicio.EliminarAsync(id) ? Ok(new { mensaje = "Eliminado correctamente" }) : NotFound(new { mensaje = "No encontrado" });

    [HttpPost("{id:guid}/ajustar-existencias")]
    public async Task<IActionResult> AjustarExistencias(Guid id, [FromBody] AjustarExistenciasSolicitud req, CancellationToken ct)
    {
        var (ok, nuevasExistencias, _) = await servicio.AjustarExistenciasAsync(id, req.Ajuste, ct);
        if (!ok) return Conflict(new { codigo = "STOCK_INSUFICIENTE", mensaje = "Stock insuficiente" });
        return Ok(new { mensaje = "Stock ajustado", stock = nuevasExistencias });
    }
}

public record AjustarExistenciasSolicitud(int Ajuste, string? Razon);