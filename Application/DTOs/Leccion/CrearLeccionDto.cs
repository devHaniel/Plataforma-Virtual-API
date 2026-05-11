namespace Application.DTOs.Leccion;

public class CrearLeccionDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? VideoUrl { get; set; }
    public string? DuracionMinutos { get; set; }
}
