namespace Domain.Entities;

public class Modulo
{
    public int Id { get; set; }
    public int CursoId { get; set; }
    public Curso Curso { get; set; } = null!;
    public string Titulo {get; set; } = string.Empty;
    public string Descripcion {get; set; } = string.Empty;
    public ICollection<Leccion> Lecciones { get; set; } = [];
}
