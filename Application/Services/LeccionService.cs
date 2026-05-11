using Application.DTOs.Leccion;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class LeccionService
{
     private readonly ILeccionRepository _leccionRepository;
    private readonly IModuloRepository _moduloRepository;
    private readonly ICursoRepository _cursoRepository;

    public LeccionService(
        ILeccionRepository leccionRepository,
        IModuloRepository moduloRepository,
        ICursoRepository cursoRepository)
    {
        _leccionRepository = leccionRepository;
        _moduloRepository = moduloRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<List<LeccionDto>> ObtenerPorModuloAsync(int moduloId)
    {
        var modulo = await _moduloRepository.ObtenerPorIdAsync(moduloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var lecciones = await _leccionRepository.ObtenerPorModuloAsync(moduloId);

        return lecciones
            .Select(l => new LeccionDto
            {
                Id = l.Id,
                ModuloId = l.ModuloId,
                Titulo = l.Titulo,
                VideoUrl = l.VideoUrl,
                DuracionMinutos = l.DuracionMinutos,
            })
            .ToList();
    }

    public async Task<LeccionDto> ObtenerPorIdAsync(int id)
    {
        var leccion = await _leccionRepository.ObtenerPorIdAsync(id);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        return new LeccionDto
        {
            Id = leccion.Id,
            ModuloId = leccion.ModuloId,
            Titulo = leccion.Titulo,
            VideoUrl = leccion.VideoUrl,
            DuracionMinutos = leccion.DuracionMinutos,
        };
    }

    public async Task<LeccionDto> CrearAsync(
        int moduloId,
        CrearLeccionDto dto,
        int usuarioId,
        string rol)
    {
        var modulo = await _moduloRepository.ObtenerPorIdAsync(moduloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para crear lecciones en este módulo.");

        var leccion = new Leccion
        {
            ModuloId = moduloId,
            Titulo = dto.Titulo,
            VideoUrl = dto.VideoUrl,
            DuracionMinutos = dto.DuracionMinutos,
        };

        await _leccionRepository.CrearAsync(leccion);

        return new LeccionDto
        {
            Id = leccion.Id,
            ModuloId = leccion.ModuloId,
            Titulo = leccion.Titulo,
            VideoUrl = leccion.VideoUrl,
            DuracionMinutos = leccion.DuracionMinutos,
        };
    }

    public async Task ActualizarAsync(
        int id,
        ActualizarLeccionDto dto,
        int usuarioId,
        string rol)
    {
        var leccion = await _leccionRepository.ObtenerPorIdAsync(id);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para actualizar esta lección.");

        leccion.Titulo = dto.Titulo;
        leccion.VideoUrl = dto.VideoUrl;
        leccion.DuracionMinutos = dto.DuracionMinutos;

        await _leccionRepository.ActualizarAsync(leccion);
    }

    public async Task EliminarAsync(int id, int usuarioId, string rol)
    {
        var leccion = await _leccionRepository.ObtenerPorIdAsync(id);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para eliminar esta lección.");

        await _leccionRepository.EliminarAsync(leccion);
    }
}
