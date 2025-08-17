using Microsoft.EntityFrameworkCore;
using ProductosAplicacion.Abstracciones;
using ProductosAplicacion.Errores;
using ProductosDominio.Entidades;
using ProductosInfraestructura.Persistencia;

namespace ProductosInfraestructura.Servicios;

public class ProductoServicio(ContextoProductos db) : IProductoServicio
{
    public async Task<IEnumerable<Producto>> ListarAsync(string? nombre, string? categoria) =>
        await db.Productos
            .Where(p => (nombre == null || EF.Functions.ILike(p.Nombre, $"%{nombre}%")) &&
                        (categoria == null || p.Categoria == categoria))
            .OrderBy(p => p.Nombre)
            .ToListAsync();

    public Task<Producto?> ObtenerPorIdAsync(Guid id) => db.Productos.FindAsync(id).AsTask();

    public async Task<Producto> CrearAsync(Producto p)
    {
        if (p is null) throw new ArgumentNullException(nameof(p));

        var nombre = (p.Nombre ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(p.Nombre));

        // Validación de duplicado (case-insensitive)
        var existe = await db.Productos
            .AnyAsync(x => x.Nombre.ToLower() == nombre.ToLower());

        if (existe)
            throw new NombreProductoDuplicadoException(nombre);

        // Alta
        p.Id = Guid.NewGuid();
        p.Nombre = nombre;
        p.FechaCreacion = DateTime.UtcNow;
        p.FechaActualizacion = null;

        db.Productos.Add(p);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (EsViolacionUnica(ex))
        {
            // Por si dos requests concurrentes pasan el AnyAsync al mismo tiempo.
            throw new NombreProductoDuplicadoException(nombre);
        }

        return p;
    }

    public async Task<bool> ActualizarAsync(Guid id, Producto p)
    {
        var existente = await db.Productos.FindAsync(id);
        if (existente is null) return false;

        existente.Nombre = p.Nombre;
        existente.Descripcion = p.Descripcion;
        existente.Categoria = p.Categoria;
        existente.UrlImagen = p.UrlImagen;
        existente.Precio = p.Precio;
        existente.Existencias = p.Existencias;
        existente.FechaActualizacion = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var existente = await db.Productos.FindAsync(id);
        if (existente is null) return false;
        db.Productos.Remove(existente);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<(bool ok, int? nuevoStock, string? error)> AjustarExistenciasAsync(Guid id, int ajuste, CancellationToken ct)
    {
        var sql = """
            
            UPDATE productos
            SET existencias = existencias + @ajuste, fecha_actualizacion = NOW()
            WHERE id = @id AND (existencias + @ajuste) >= 0
            RETURNING existencias;
            
            """;

        await using var conn = db.Database.GetDbConnection();
        await conn.OpenAsync(ct);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.Value = id; cmd.Parameters.Add(pId);
        var pAjuste = cmd.CreateParameter(); pAjuste.ParameterName = "@ajuste"; pAjuste.Value = ajuste; cmd.Parameters.Add(pAjuste);

        var result = await cmd.ExecuteScalarAsync(ct);
        if (result is DBNull or null)
            return (false, null, "STOCK_INSUFICIENTE");

        return (true, Convert.ToInt32(result), null);
    }

    private static bool EsViolacionUnica(DbUpdateException ex)
        => ex.InnerException is Npgsql.PostgresException pg
           && pg.SqlState == "23505";
}
