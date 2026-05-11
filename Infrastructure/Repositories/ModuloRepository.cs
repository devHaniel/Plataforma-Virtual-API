using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class ModuloRepository : IModuloRepository
{
    private readonly AppDbContext _context;

    public ModuloRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Modulo>> ObtenerPorCursoAsync(int cursoId)
    {
        return await _context.Modulos
            .Where(m => m.CursoId == cursoId)
            .Include(m => m.Lecciones)
            .ToListAsync();
    }

    public async Task<Modulo?> ObtenerPorIdAsync(int id)
    {
        return await _context.Modulos
            .Include(m => m.Lecciones)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task CrearAsync(Modulo modulo)
    {
        await _context.Modulos.AddAsync(modulo);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Modulo modulo)
    {
        _context.Modulos.Update(modulo);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Modulo modulo)
    {
        _context.Modulos.Remove(modulo);
        await _context.SaveChangesAsync();
    }
}