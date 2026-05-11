using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ILeccionRepository
{
    Task<List<Leccion>> ObtenerPorModuloAsync(int moduloId);
    Task<Leccion?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Leccion leccion);
    Task ActualizarAsync(Leccion leccion);
    Task EliminarAsync(Leccion leccion);
}