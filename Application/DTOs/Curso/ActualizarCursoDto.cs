namespace Application.DTOs.Curso;

public class ActualizarCursoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
}