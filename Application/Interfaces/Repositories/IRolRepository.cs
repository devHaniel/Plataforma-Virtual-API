using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IRolRepository
{
    Task<List<Rol>> ObtenerTodosAsync();
    Task<Rol?> ObtenerPorIdAsync(int id);
    Task<Rol?> ObtenerPorNombreAsync(string nombre);
}