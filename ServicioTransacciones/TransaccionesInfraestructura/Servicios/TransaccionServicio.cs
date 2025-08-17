using Microsoft.EntityFrameworkCore;
using TransaccionesAplicacion.Abstracciones;
using TransaccionesAplicacion.Externos;
using TransaccionesDominio.Entidades;
using TransaccionesInfraestructura.Persistencia;

namespace TransaccionesInfraestructura.Servicios;

public class TransaccionServicio(ContextoTransacciones db, IProductosClient productos) : ITransaccionServicio
{
    public async Task<Transaccion> RegistrarAsync(Transaccion t, CancellationToken ct)
    {
        // Validaciones de negocio
        if (t.Cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(t.Cantidad));
        if (t.PrecioUnitario < 0) throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(t.PrecioUnitario));
        if (string.IsNullOrWhiteSpace(t.Tipo) || (t.Tipo != "compra" && t.Tipo != "venta"))
            throw new ArgumentException("Tipo invalido. Use 'compra' o 'venta'.", nameof(t.Tipo));

        // Ajuste de existencias en Productos
        var cambio = t.Tipo == "compra" ? +t.Cantidad : -t.Cantidad;
        var (ok, _, _, msg) = await productos.AjustarExistenciasAsync(t.ProductoId, cambio, ct);
        if (!ok) throw new InvalidOperationException(msg ?? "No fue posible ajustar existencias en Productos.");

        // Persistir
        t.Id = Guid.NewGuid();
        if (t.Fecha == default) t.Fecha = DateTime.UtcNow;

        db.Transacciones.Add(t);
        await db.SaveChangesAsync(ct);  

        return t;
    }

    public Task<Transaccion?> ObtenerPorIdAsync(Guid id)
        => db.Transacciones.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Transaccion>> ListarAsync(Guid? productoId, string? tipo, DateTime? desde, DateTime? hasta)
    {
        var q = db.Transacciones.AsQueryable();
        if (productoId is { } pid) q = q.Where(x => x.ProductoId == pid);
        if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(x => x.Tipo == tipo);
        if (desde is { } d) q = q.Where(x => x.Fecha >= d);
        if (hasta is { } h) q = q.Where(x => x.Fecha <= h);

        return await q.OrderByDescending(x => x.Fecha).ToListAsync();
    }

    public async Task<HistorialResultado> HistorialAsync(
        int page, int pageSize, Guid? productoId, string? tipo, DateTime? desde, DateTime? hasta, CancellationToken ct)
    {
        // join transacciones + productos (nombre y existencias)
        var q =
            from t in db.Transacciones.AsNoTracking()
            join p in db.Productos.AsNoTracking() on t.ProductoId equals p.Id
            select new { t, p };

        if (productoId.HasValue) q = q.Where(x => x.t.ProductoId == productoId.Value);
        if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(x => x.t.Tipo == tipo);
        if (desde.HasValue) q = q.Where(x => x.t.Fecha >= desde.Value);
        if (hasta.HasValue) q = q.Where(x => x.t.Fecha <= hasta.Value);

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(x => x.t.Fecha)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new HistorialItem(
                x.t.Id,
                x.t.Fecha,
                x.t.Tipo,
                x.t.ProductoId,
                x.p.Nombre,
                x.t.Cantidad,
                x.t.PrecioUnitario,
                x.t.PrecioTotal,
                x.p.Existencias
            ))
            .ToListAsync(ct);

        return new HistorialResultado(total, page, pageSize, items);
    }

    public async Task<bool> ActualizarObservacionAsync(Guid id, string? observacion, CancellationToken ct)
    {
        var filas = await db.Transacciones
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(t => t.Observacion, observacion),
                ct);

        return filas > 0;
    }
}
