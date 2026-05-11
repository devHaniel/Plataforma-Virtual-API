using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EntregaRepository : IEntregaRepository
{
    private readonly AppDbContext _context;

    public EntregaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Entrega>> ObtenerPorTareaAsync(int tareaId)
    {
        return await _context.Entregas
            .Where(e => e.TareaId == tareaId)
            .Include(e => e.Usuario)
            .ToListAsync();
    }

    public async Task<Entrega?> ObtenerPorIdAsync(int id)
    {
        return await _context.Entregas
            .Include(e => e.Usuario)
            .Include(e => e.Tarea)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Entrega?> ObtenerPorTareaYUsuarioAsync(int tareaId, int usuarioId)
    {
        return await _context.Entregas
            .FirstOrDefaultAsync(e => e.TareaId == tareaId && e.UsuarioId == usuarioId);
    }

    public async Task CrearAsync(Entrega entrega)
    {
        await _context.Entregas.AddAsync(entrega);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Entrega entrega)
    {
        _context.Entregas.Update(entrega);
        await _context.SaveChangesAsync();
    }
}