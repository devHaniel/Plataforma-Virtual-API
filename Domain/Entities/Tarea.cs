namespace Domain.Entities;

public class Tarea
{
    public int Id { get; set; }
    public int LeccionId { get; set; }
    public Leccion Leccion { get; set; } = null!;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaAsignacion {get; set;}
    public DateTime FechaEntrega {get; set;}
    public decimal PuntajeMaximo {get; set;}
    public ICollection<Entrega> Entregas { get; set; } = [];
}
