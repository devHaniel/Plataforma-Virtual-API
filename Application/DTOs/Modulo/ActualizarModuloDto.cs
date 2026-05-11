namespace Application.DTOs.Modulo;

public class ActualizarModuloDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
}