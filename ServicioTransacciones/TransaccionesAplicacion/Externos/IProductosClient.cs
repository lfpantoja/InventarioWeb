namespace TransaccionesAplicacion.Externos;

public interface IProductosClient
{
    Task<(bool ok, int? nuevasExistencias, string? errorCodigo, string? errorMensaje)>
        AjustarExistenciasAsync(Guid productoId, int cambio, CancellationToken ct);
}