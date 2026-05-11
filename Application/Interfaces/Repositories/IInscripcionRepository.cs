using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IInscripcionRepository
{
    Task<List<Inscripcion>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<List<Inscripcion>> ObtenerPorCursoAsync(int cursoId);
    Task<Inscripcion?> ObtenerAsync(int usuarioId, int cursoId);
    Task CrearAsync(Inscripcion inscripcion);
    Task EliminarAsync(Inscripcion inscripcion);
}