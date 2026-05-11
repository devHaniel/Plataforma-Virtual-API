using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class CursoRepository : ICursoRepository
{
    private readonly AppDbContext _context;

    public CursoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Curso>> ObtenerTodosAsync()
    {
        return await _context.Cursos
            .Include(c => c.Profesor)
            .ToListAsync();
    }

    public async Task<Curso?> ObtenerPorIdAsync(int id)
    {
        return await _context.Cursos
            .Include(c => c.Profesor)
            .Include(c => c.Modulos)
            .Include(c => c.Inscripciones)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CrearAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Curso curso)
    {
        _context.Cursos.Update(curso);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Curso curso)
    {
        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Cursos.AnyAsync(c => c.Id == id);
    }
}