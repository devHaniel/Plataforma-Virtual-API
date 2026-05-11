namespace Application.DTOs.Curso;

public class CrearCursoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
}