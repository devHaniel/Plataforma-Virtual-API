using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuloRepository
{
    Task<List<Modulo>> ObtenerPorCursoAsync(int cursoId);
    Task<Modulo?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Modulo modulo);
    Task ActualizarAsync(Modulo modulo);
    Task EliminarAsync(Modulo modulo);
}
