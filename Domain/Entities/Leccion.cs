namespace Domain.Entities;

public class Leccion
{
    public int Id { get; set; }
    public int ModuloId { get; set; }
    public Modulo Modulo { get; set; } = null!;
    public string Titulo {get; set; } = string.Empty;
    public string Descripcion {get; set; } = string.Empty;
    public string VideoUrl {get; set; } = string.Empty;
    public string DuracionMinutos {get; set;} = string.Empty;
    public ICollection<Tarea> Tareas { get; set; } = [];
}
