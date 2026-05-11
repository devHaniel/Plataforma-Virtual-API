using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {

    }
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Modulo> Modulos => Set<Modulo>();
    public DbSet<Leccion> Lecciones => Set<Leccion>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
    public DbSet<Tarea> Tareas => Set<Tarea>();
    public DbSet<Entrega> Entregas => Set<Entrega>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ROLES
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Roles");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(r => r.Nombre)
                .IsUnique();

            entity.HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // USUARIOS
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Activo)
                .HasDefaultValue(true);

            entity.Property(u => u.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            entity.HasMany(u => u.Cursos)
                .WithOne(c => c.Profesor)
                .HasForeignKey(c => c.ProfesorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.Inscripciones)
                .WithOne(i => i.Usuario)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.Entregas)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CURSOS
        modelBuilder.Entity<Curso>(entity =>
        {
            entity.ToTable("Cursos");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Descripcion)
                .HasMaxLength(1000);

            entity.Property(c => c.ImagenUrl)
                .HasMaxLength(500);

            entity.Property(c => c.Activo)
                .HasDefaultValue(true);

            entity.Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(c => c.Profesor)
                .WithMany(u => u.Cursos)
                .HasForeignKey(c => c.ProfesorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Modulos)
                .WithOne(m => m.Curso)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Inscripciones)
                .WithOne(i => i.Curso)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MODULOS
        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.ToTable("Modulos");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Titulo)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(m => m.Descripcion)
                .HasMaxLength(1000);

            entity.HasOne(m => m.Curso)
                .WithMany(c => c.Modulos)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.Lecciones)
                .WithOne(l => l.Modulo)
                .HasForeignKey(l => l.ModuloId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // LECCIONES
        modelBuilder.Entity<Leccion>(entity =>
        {
            entity.ToTable("Lecciones");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.Titulo)
                .IsRequired()
                .HasMaxLength(150);


            entity.Property(l => l.Descripcion)
                .HasColumnType("nvarchar(max)");

            entity.Property(l => l.VideoUrl)
                .HasMaxLength(500);

            entity.Property(l => l.DuracionMinutos)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasOne(l => l.Modulo)
                .WithMany(m => m.Lecciones)
                .HasForeignKey(l => l.ModuloId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(l => l.Tareas)
                .WithOne(t => t.Leccion)
                .HasForeignKey(t => t.LeccionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // INSCRIPCIONES
        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.ToTable("Inscripciones");

            entity.HasKey(i => i.Id);

            entity.Property(i => i.FechaInscripcion)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(i => i.Usuario)
                .WithMany(u => u.Inscripciones)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Curso)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(i => new { i.UsuarioId, i.CursoId })
                .IsUnique();
        });

        // TAREAS
        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.ToTable("Tareas");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Descripcion)
                .HasColumnType("nvarchar(max)");

            entity.Property(t => t.FechaAsignacion)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(t => t.PuntajeMaximo)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(100);

            entity.HasOne(t => t.Leccion)
                .WithMany(l => l.Tareas)
                .HasForeignKey(t => t.LeccionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(t => t.Entregas)
                .WithOne(e => e.Tarea)
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ENTREGAS
        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.ToTable("Entregas");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.ArchivoUrl)
                .HasMaxLength(500);

            entity.Property(e => e.Comentario)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(5,2)");


            entity.HasOne(e => e.Tarea)
                .WithMany(t => t.Entregas)
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Entregas)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.TareaId, e.UsuarioId })
                .IsUnique();
        });

    }
}
