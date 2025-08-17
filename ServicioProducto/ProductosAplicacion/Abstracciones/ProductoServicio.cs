using ProductosDominio.Entidades;

namespace ProductosAplicacion.Abstracciones
{
    public interface IProductoServicio
    {
        Task<IEnumerable<Producto>> ListarAsync(string? nombre, string? categoria);
        Task<Producto?> ObtenerPorIdAsync(Guid id);
        Task<Producto> CrearAsync(Producto p);
        Task<bool> ActualizarAsync(Guid id, Producto p);
        Task<bool> EliminarAsync(Guid id);
        Task<(bool ok, int? nuevoStock, string? error)> AjustarExistenciasAsync(Guid id, int cantidadAjuste, CancellationToken ct);
    }
}