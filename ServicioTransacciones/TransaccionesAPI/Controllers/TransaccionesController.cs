using Microsoft.AspNetCore.Mvc;
using TransaccionesAplicacion.Abstracciones;
using TransaccionesDominio.Entidades;

namespace TransaccionesAPI.Controllers;

[ApiController]
[Route("api/transacciones")]
public class TransaccionesController(ITransaccionServicio servicio) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Transaccion t, CancellationToken ct)
    {
        try
        {
            var creada = await servicio.RegistrarAsync(t, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex) { return BadRequest(new { mensaje = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obtener(Guid id)
        => (await servicio.ObtenerPorIdAsync(id)) is { } x ? Ok(x) : NotFound();

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] Guid? productoId, [FromQuery] string? tipo,
                                            [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        => Ok(await servicio.ListarAsync(productoId, tipo, desde, hasta));

    [HttpGet("historial")]
    public async Task<IActionResult> Historial(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] Guid? productoId = null,
    [FromQuery] string? tipo = null,
    [FromQuery] string? desde = null,
    [FromQuery] string? hasta = null,
    CancellationToken ct = default)
    {
        DateTime? d = null, h = null;

        if (!string.IsNullOrWhiteSpace(desde))
        {
            // Si te llegan solo fechas (YYYY-MM-DD), las tratamos como inicio del día UTC
            var dtmp = DateTime.Parse(desde); // o DateOnly.Parse(desde)
            d = DateTime.SpecifyKind(dtmp.Date, DateTimeKind.Utc); // 00:00:00Z
        }

        if (!string.IsNullOrWhiteSpace(hasta))
        {
            var htmp = DateTime.Parse(hasta);
            // fin del día UTC (23:59:59.999...)
            h = DateTime.SpecifyKind(htmp.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
        }

        // sanity check
        if (d.HasValue && h.HasValue && d > h)
            return BadRequest(new { mensaje = "'desde' no puede ser mayor que 'hasta'" });

        var res = await servicio.HistorialAsync(page, pageSize, productoId, tipo, d, h, ct);
        return Ok(res);
    }

}
