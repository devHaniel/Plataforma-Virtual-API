using Application.DTOs.Tarea;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class TareaService
{
     private readonly ITareaRepository _tareaRepository;
    private readonly ILeccionRepository _leccionRepository;
    private readonly IModuloRepository _moduloRepository;
    private readonly ICursoRepository _cursoRepository;

    public TareaService(
        ITareaRepository tareaRepository,
        ILeccionRepository leccionRepository,
        IModuloRepository moduloRepository,
        ICursoRepository cursoRepository)
    {
        _tareaRepository = tareaRepository;
        _leccionRepository = leccionRepository;
        _moduloRepository = moduloRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<List<TareaDto>> ObtenerPorLeccionAsync(int leccionId)
    {
        var leccion = await _leccionRepository.ObtenerPorIdAsync(leccionId);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var tareas = await _tareaRepository.ObtenerPorLeccionAsync(leccionId);

        return tareas.Select(t => new TareaDto
        {
            Id = t.Id,
            LeccionId = t.LeccionId,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            FechaAsignacion = t.FechaAsignacion,
            FechaEntrega = t.FechaEntrega,
            PuntajeMaximo = t.PuntajeMaximo
        }).ToList();
    }

    public async Task<TareaDto> ObtenerPorIdAsync(int id)
    {
        var tarea = await _tareaRepository.ObtenerPorIdAsync(id);

        if (tarea is null)
            throw new Exception("Tarea no encontrada.");

        return MapearTareaDto(tarea);
    }

    public async Task<TareaDto> CrearAsync(
        int leccionId,
        CrearTareaDto dto,
        int usuarioId,
        string rol)
    {
        var leccion = await _leccionRepository.ObtenerPorIdAsync(leccionId);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para crear tareas en esta lección.");

        var tarea = new Tarea
        {
            LeccionId = leccionId,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            FechaAsignacion = DateTime.Now,
            FechaEntrega = dto.FechaEntrega,
            PuntajeMaximo = dto.PuntajeMaximo
        };

        await _tareaRepository.CrearAsync(tarea);

        return MapearTareaDto(tarea);
    }

    public async Task ActualizarAsync(
        int id,
        ActualizarTareaDto dto,
        int usuarioId,
        string rol)
    {
        var tarea = await _tareaRepository.ObtenerPorIdAsync(id);

        if (tarea is null)
            throw new Exception("Tarea no encontrada.");

        var leccion = await _leccionRepository.ObtenerPorIdAsync(tarea.LeccionId);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para actualizar esta tarea.");

        tarea.Titulo = dto.Titulo;
        tarea.Descripcion = dto.Descripcion;
        tarea.FechaEntrega = dto.FechaEntrega;
        tarea.PuntajeMaximo = dto.PuntajeMaximo;

        await _tareaRepository.ActualizarAsync(tarea);
    }

    public async Task EliminarAsync(int id, int usuarioId, string rol)
    {
        var tarea = await _tareaRepository.ObtenerPorIdAsync(id);

        if (tarea is null)
            throw new Exception("Tarea no encontrada.");

        var leccion = await _leccionRepository.ObtenerPorIdAsync(tarea.LeccionId);

        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);

        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para eliminar esta tarea.");

        await _tareaRepository.EliminarAsync(tarea);
    }

    private static TareaDto MapearTareaDto(Tarea tarea)
    {
        return new TareaDto
        {
            Id = tarea.Id,
            LeccionId = tarea.LeccionId,
            Titulo = tarea.Titulo,
            Descripcion = tarea.Descripcion,
            FechaAsignacion = tarea.FechaAsignacion,
            FechaEntrega = tarea.FechaEntrega,
            PuntajeMaximo = tarea.PuntajeMaximo
        };
    }
}
