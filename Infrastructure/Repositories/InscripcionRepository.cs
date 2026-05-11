using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InscripcionRepository : IInscripcionRepository
{
    private readonly AppDbContext _context;

    public InscripcionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Inscripcion>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _context.Inscripciones
            .Where(i => i.UsuarioId == usuarioId)
            .Include(i => i.Curso)
            .ToListAsync();
    }

    public async Task<List<Inscripcion>> ObtenerPorCursoAsync(int cursoId)
    {
        return await _context.Inscripciones
            .Where(i => i.CursoId == cursoId)
            .Include(i => i.Usuario)
            .ToListAsync();
    }

    public async Task<Inscripcion?> ObtenerAsync(int usuarioId, int cursoId)
    {
        return await _context.Inscripciones
            .FirstOrDefaultAsync(i => i.UsuarioId == usuarioId && i.CursoId == cursoId);
    }

    public async Task CrearAsync(Inscripcion inscripcion)
    {
        await _context.Inscripciones.AddAsync(inscripcion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Inscripcion inscripcion)
    {
        _context.Inscripciones.Remove(inscripcion);
        await _context.SaveChangesAsync();
    }
}