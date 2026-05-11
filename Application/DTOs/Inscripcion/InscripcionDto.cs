namespace Application.DTOs.Inscripcion;

public class InscripcionDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public int CursoId { get; set; }
    public string Curso { get; set; } = string.Empty;
    public DateTime FechaInscripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal ProgresoPorcentaje { get; set; }
}