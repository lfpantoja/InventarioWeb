namespace TransaccionesInfraestructura.Persistencia
{
    public class Producto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = default!;
        public int Existencias { get; set; }
    }
}
