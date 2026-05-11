namespace Domain.Entities;

public class Entrega
{
    public int Id { get; set; }
    public int TareaId { get; set; }
    public Tarea Tarea { get; set; } = null!;
    public int UsuarioId { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public decimal Calificacion { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public DateTime FechaEntrega { get; set; }
    public string ArchivoUrl { get; set; } = string.Empty;
}
