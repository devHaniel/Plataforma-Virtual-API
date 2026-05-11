namespace Application.DTOs.Tarea;

public class TareaDto
{
    public int Id { get; set; }
    public int LeccionId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public DateTime FechaEntrega { get; set; }
    public decimal PuntajeMaximo { get; set; }
}