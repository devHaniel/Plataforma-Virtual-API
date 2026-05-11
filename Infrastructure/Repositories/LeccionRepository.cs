using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LeccionRepository : ILeccionRepository
{
    private readonly AppDbContext _context;

    public LeccionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Leccion>> ObtenerPorModuloAsync(int moduloId)
    {
        return await _context.Lecciones
            .Where(l => l.ModuloId == moduloId)
            .Include(l => l.Tareas)
            .ToListAsync();
    }

    public async Task<Leccion?> ObtenerPorIdAsync(int id)
    {
        return await _context.Lecciones
            .Include(l => l.Tareas)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task CrearAsync(Leccion leccion)
    {
        await _context.Lecciones.AddAsync(leccion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Leccion leccion)
    {
        _context.Lecciones.Update(leccion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Leccion leccion)
    {
        _context.Lecciones.Remove(leccion);
        await _context.SaveChangesAsync();
    }
}