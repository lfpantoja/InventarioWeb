using Microsoft.EntityFrameworkCore;
using ProductosDominio.Entidades;

namespace ProductosInfraestructura.Persistencia
{
    public class ContextoProductos : DbContext
    {
        public ContextoProductos(DbContextOptions<ContextoProductos> options)
            : base(options) { }

        public DbSet<Producto> Productos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("productos");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Categoria).HasColumnName("categoria");
                entity.Property(e => e.UrlImagen).HasColumnName("url_imagen");
                entity.Property(e => e.Precio).HasColumnName("precio").HasPrecision(12, 2); ;
                entity.Property(e => e.Existencias).HasColumnName("existencias");
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
                entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            });
        }
    }
}
