using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICursoRepository
{
    Task<List<Curso>> ObtenerTodosAsync();
    Task<Curso?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Curso curso);
    Task ActualizarAsync(Curso curso);
    Task EliminarAsync(Curso curso);
    Task<bool> ExisteAsync(int id);
}