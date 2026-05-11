using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IEntregaRepository
{
    Task<List<Entrega>> ObtenerPorTareaAsync(int tareaId);
    Task<Entrega?> ObtenerPorIdAsync(int id);
    Task<Entrega?> ObtenerPorTareaYUsuarioAsync(int tareaId, int usuarioId);
    Task CrearAsync(Entrega entrega);
    Task ActualizarAsync(Entrega entrega);
}