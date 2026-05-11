using Application.DTOs.Modulo;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;


public class ModuloService
{
    private readonly IModuloRepository _moduloRepository;
    private readonly ICursoRepository _cursoRepository;

    public ModuloService(
        IModuloRepository moduloRepository,
        ICursoRepository cursoRepository)
    {
        _moduloRepository = moduloRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<List<ModuloDto>> ObtenerPorCursoAsync(int cursoId)
    {
        var cursoExiste = await _cursoRepository.ExisteAsync(cursoId);

        if (!cursoExiste)
            throw new Exception("El curso no existe.");

        var modulos = await _moduloRepository.ObtenerPorCursoAsync(cursoId);

        return modulos
            .Select(m => new ModuloDto
            {
                Id = m.Id,
                Titulo = m.Titulo,
                Descripcion = m.Descripcion,
                CursoId = m.CursoId
            })
            .ToList();
    }

    public async Task<ModuloDto> ObtenerPorIdAsync(int id)
    {
        var modulo = await _moduloRepository.ObtenerPorIdAsync(id);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        return new ModuloDto
        {
            Id = modulo.Id,
            Titulo = modulo.Titulo,
            Descripcion = modulo.Descripcion,
            CursoId = modulo.CursoId
        };
    }

    public async Task<ModuloDto> CrearAsync(
        int cursoId,
        CrearModuloDto dto,
        int usuarioId,
        string rol)
    {
        var curso = await _cursoRepository.ObtenerPorIdAsync(cursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para crear módulos en este curso.");

        var modulo = new Modulo
        {
            CursoId = cursoId,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
        };

        await _moduloRepository.CrearAsync(modulo);

        return new ModuloDto
        {
            Id = modulo.Id,
            Titulo = modulo.Titulo,
            Descripcion = modulo.Descripcion,
            CursoId = modulo.CursoId
        };
    }

    public async Task ActualizarAsync(
        int id,
        ActualizarModuloDto dto,
        int usuarioId,
        string rol)
    {
        var modulo = await _moduloRepository.ObtenerPorIdAsync(id);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para actualizar este módulo.");

        modulo.Titulo = dto.Titulo;
        modulo.Descripcion = dto.Descripcion;

        await _moduloRepository.ActualizarAsync(modulo);
    }

    public async Task EliminarAsync(int id, int usuarioId, string rol)
    {
        var modulo = await _moduloRepository.ObtenerPorIdAsync(id);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para eliminar este módulo.");

        await _moduloRepository.EliminarAsync(modulo);
    }
}