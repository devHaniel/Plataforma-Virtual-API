namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public ICollection<Inscripcion> Inscripciones { get; set; } = [];
    public ICollection<Curso> Cursos { get; set; } = [];
    public ICollection<Entrega> Entregas { get; set; } = [];
}
