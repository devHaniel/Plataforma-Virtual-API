namespace Application.DTOs.Tarea;

public class CrearTareaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaEntrega { get; set; }
    public decimal PuntajeMaximo { get; set; } = 100;
}
