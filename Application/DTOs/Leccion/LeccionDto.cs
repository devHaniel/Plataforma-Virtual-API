namespace Application.DTOs.Leccion;

public class LeccionDto
{
    public int Id { get; set; }
    public int ModuloId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? VideoUrl { get; set; }
    public string? DuracionMinutos { get; set; }
}