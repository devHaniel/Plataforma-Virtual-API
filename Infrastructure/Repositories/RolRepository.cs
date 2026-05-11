using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class RolRepository : IRolRepository
{
    private readonly AppDbContext _context;

    public RolRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rol>> ObtenerTodosAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Rol?> ObtenerPorIdAsync(int id)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rol?> ObtenerPorNombreAsync(string nombre)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
    }
}