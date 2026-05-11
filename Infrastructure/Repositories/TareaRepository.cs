using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly AppDbContext _context;

    public TareaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tarea>> ObtenerPorLeccionAsync(int leccionId)
    {
        return await _context.Tareas
            .Where(t => t.LeccionId == leccionId)
            .Include(t => t.Entregas)
            .ToListAsync();
    }

    public async Task<Tarea?> ObtenerPorIdAsync(int id)
    {
        return await _context.Tareas
            .Include(t => t.Entregas)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task CrearAsync(Tarea tarea)
    {
        await _context.Tareas.AddAsync(tarea);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Tarea tarea)
    {
        _context.Tareas.Update(tarea);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Tarea tarea)
    {
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
    }
}