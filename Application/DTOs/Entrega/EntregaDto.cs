namespace Application.DTOs.Entrega;

public class EntregaDto
{
    public int Id { get; set; }
    public int TareaId { get; set; }
    public int UsuarioId { get; set; }
    public string Estudiante { get; set; } = string.Empty;
    public string? ArchivoUrl { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaEntrega { get; set; }
    public decimal? Calificacion { get; set; }
    public string? Retroalimentacion { get; set; }
}