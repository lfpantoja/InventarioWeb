namespace TransaccionesDominio.Entidades;

public class Transaccion
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Tipo { get; set; } = default!;           // 'compra' | 'venta'
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PrecioTotal { get; set; }               // DB la calcula; lo mapeamos como computada
    public string? Observacion { get; set; }
}