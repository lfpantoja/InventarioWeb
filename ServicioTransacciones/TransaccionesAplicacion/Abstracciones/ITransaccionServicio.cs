using TransaccionesDominio.Entidades;

namespace TransaccionesAplicacion.Abstracciones;

public interface ITransaccionServicio
{
    Task<Transaccion> RegistrarAsync(Transaccion t, CancellationToken ct);
    Task<Transaccion?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<Transaccion>> ListarAsync(Guid? productoId, string? tipo, DateTime? desde, DateTime? hasta);
    Task<HistorialResultado> HistorialAsync(
        int page, int pageSize,
        Guid? productoId, string? tipo,
        DateTime? desde, DateTime? hasta,
        CancellationToken ct);
}

public record HistorialItem(
    Guid Id,
    DateTime Fecha,
    string Tipo,
    Guid ProductoId,
    string NombreProducto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal PrecioTotal,
    int StockActual
);

public record HistorialResultado(
    int Total,
    int Page,
    int PageSize,
    IEnumerable<HistorialItem> Items
);