namespace Domain.Entities;

public class Curso
{
    public int Id {get; set;}
    public string Nombre {get; set;} = string.Empty;
    public string Descripcion {get; set;} = string.Empty;
    public string ImagenUrl {get; set;} = string.Empty;
    public int ProfesorId {get; set;}
    public Usuario Profesor {get; set;} = null!;
    public bool Activo {get; set;}
    public DateTime FechaCreacion {get; set;}
    public ICollection<Modulo> Modulos { get; set; } = [];
    public ICollection<Inscripcion> Inscripciones { get; set; } = [];

}
