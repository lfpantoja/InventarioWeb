using System.Net;
using System.Net.Http.Json;
using TransaccionesAplicacion.Externos;

namespace TransaccionesInfraestructura.Externos;

public class ProductosClient(HttpClient http) : IProductosClient
{
    private record Req(int ajuste, string? razon);
    private record Ok(int stock);
    private record Err(string? codigo, string? mensaje);

    public async Task<(bool ok, int? nuevasExistencias, string? errorCodigo, string? errorMensaje)>
        AjustarExistenciasAsync(Guid productoId, int cambio, CancellationToken ct)
    {
        var url = $"api/productos/{productoId}/ajustar-existencias";
        // 👇 el nombre de propiedad debe ser "ajuste"
        using var resp = await http.PostAsJsonAsync(url, new Req(cambio, cambio >= 0 ? "compra" : "venta"), ct);

        if (resp.IsSuccessStatusCode)
        {
            var ok = await resp.Content.ReadFromJsonAsync<Ok>(cancellationToken: ct);
            return (true, ok?.stock, null, null);
        }

        if (resp.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.BadRequest)
        {
            var err = await resp.Content.ReadFromJsonAsync<Err>(cancellationToken: ct);
            return (false, null, err?.codigo, err?.mensaje ?? "conflicto al ajustar existencias");
        }

        var txt = await resp.Content.ReadAsStringAsync(ct);
        return (false, null, "ERROR_PRODUCTOS", $"HTTP {(int)resp.StatusCode}: {txt}");
    }
}
