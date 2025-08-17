using Microsoft.EntityFrameworkCore;
using TransaccionesDominio.Entidades;

namespace TransaccionesInfraestructura.Persistencia;

public class ContextoTransacciones(DbContextOptions<ContextoTransacciones> options) : DbContext(options)
{
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaccion>(e =>
        {
            e.ToTable("transacciones");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Fecha).HasColumnName("fecha");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.Cantidad).HasColumnName("cantidad");
            e.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(12, 2);

            // columna computada en DB
            e.Property(x => x.PrecioTotal)
             .HasColumnName("precio_total")
             .HasPrecision(14, 2)
             .HasComputedColumnSql("cantidad * precio_unitario", stored: true);

            e.Property(x => x.Observacion).HasColumnName("observacion");

            e.HasIndex(x => x.ProductoId).HasDatabaseName("ix_transacciones_producto");
            e.HasIndex(x => x.Fecha).HasDatabaseName("ix_transacciones_fecha");
            e.HasIndex(x => x.Tipo).HasDatabaseName("ix_transacciones_tipo");
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nombre).HasColumnName("nombre");
            e.Property(x => x.Existencias).HasColumnName("existencias");
        });
    }
}
