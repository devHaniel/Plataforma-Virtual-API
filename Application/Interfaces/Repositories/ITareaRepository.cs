using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ITareaRepository
{
    Task<List<Tarea>> ObtenerPorLeccionAsync(int leccionId);
    Task<Tarea?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Tarea tarea);
    Task ActualizarAsync(Tarea tarea);
    Task EliminarAsync(Tarea tarea);
}