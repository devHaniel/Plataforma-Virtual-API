namespace Domain.Entities;

public class Inscripcion
{
    public int Id {get; set;}
    public int UsuarioId {get; set;}
    public int CursoId {get; set;}
    public Usuario Usuario {get; set;} = null!;
    public Curso Curso {get; set; } = null!;
    public DateTime FechaInscripcion {get; set;}
}
