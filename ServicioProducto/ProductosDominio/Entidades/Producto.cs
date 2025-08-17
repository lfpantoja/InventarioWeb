namespace ProductosDominio.Entidades
{
    public class Producto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string? Descripcion { get; set; }
        public string Categoria { get; set; } = default!;
        public string? UrlImagen { get; set; }
        public decimal Precio { get; set; }
        public int Existencias { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}