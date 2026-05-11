namespace Application.DTOs.Curso;

public class CursoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public int ProfesorId { get; set; }
    public string Profesor { get; set; } = string.Empty;
}