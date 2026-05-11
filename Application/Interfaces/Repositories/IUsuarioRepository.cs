using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IUsuarioRepository
{
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task CrearAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task EliminarAsync(Usuario usuario);
    Task<bool> ExisteAsync(int id);
}