namespace ProductosAplicacion.Errores;

public class NombreProductoDuplicadoException : Exception
{
    public NombreProductoDuplicadoException(string nombre)
        : base($"Ya existe un producto con el nombre '{nombre}'.") { }
}
