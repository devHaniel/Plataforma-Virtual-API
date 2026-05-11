using Application.DTOs.Entrega;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class EntregaService
{
    private readonly IEntregaRepository _entregaRepository;
    private readonly ITareaRepository _tareaRepository;
    private readonly ILeccionRepository _leccionRepository;
    private readonly IModuloRepository _moduloRepository;
    private readonly ICursoRepository _cursoRepository;
    private readonly IInscripcionRepository _inscripcionRepository;

    public EntregaService(
        IEntregaRepository entregaRepository,
        ITareaRepository tareaRepository,
        ILeccionRepository leccionRepository,
        IModuloRepository moduloRepository,
        ICursoRepository cursoRepository,
        IInscripcionRepository inscripcionRepository)
    {
        _entregaRepository = entregaRepository;
        _tareaRepository = tareaRepository;
        _leccionRepository = leccionRepository;
        _moduloRepository = moduloRepository;
        _cursoRepository = cursoRepository;
        _inscripcionRepository = inscripcionRepository;
    }

    public async Task<EntregaDto> EnviarAsync(
    int tareaId,
    string? comentario,
    string archivoUrl,
    int usuarioId)
    {
        var tarea = await _tareaRepository.ObtenerPorIdAsync(tareaId);

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

        var inscripcion = await _inscripcionRepository.ObtenerAsync(usuarioId, curso.Id);

        if (inscripcion is null)
            throw new Exception("No estás inscrito en este curso.");

        var entregaExistente = await _entregaRepository.ObtenerPorTareaYUsuarioAsync(
            tareaId,
            usuarioId
        );

        if (entregaExistente is not null)
            throw new Exception("Ya enviaste una entrega para esta tarea.");

        var entrega = new Entrega
        {
            TareaId = tareaId,
            UsuarioId = usuarioId,
            ArchivoUrl = archivoUrl,
            Comentario = comentario,
            FechaEntrega = DateTime.Now
        };

        await _entregaRepository.CrearAsync(entrega);

        return new EntregaDto
        {
            Id = entrega.Id,
            TareaId = entrega.TareaId,
            UsuarioId = entrega.UsuarioId,
            ArchivoUrl = entrega.ArchivoUrl,
            Comentario = entrega.Comentario,
            FechaEntrega = entrega.FechaEntrega
        };
    }

    public async Task<List<EntregaDto>> ObtenerPorTareaAsync(
        int tareaId,
        int usuarioId,
        string rol)
    {
        var tarea = await _tareaRepository.ObtenerPorIdAsync(tareaId);

        if (tarea is null)
            throw new Exception("Tarea no encontrada.");

        await ValidarProfesorDelCursoAsync(tarea, usuarioId, rol);

        var entregas = await _entregaRepository.ObtenerPorTareaAsync(tareaId);

        return entregas.Select(e => new EntregaDto
        {
            Id = e.Id,
            TareaId = e.TareaId,
            UsuarioId = e.UsuarioId,
            Estudiante = e.Usuario.Nombre,
            ArchivoUrl = e.ArchivoUrl,
            Comentario = e.Comentario,
            FechaEntrega = e.FechaEntrega,
            Calificacion = e.Calificacion,
        }).ToList();
    }

    public async Task<EntregaDto> ObtenerPorIdAsync(int id, int usuarioId, string rol)
    {
        var entrega = await _entregaRepository.ObtenerPorIdAsync(id);

        if (entrega is null)
            throw new Exception("Entrega no encontrada.");

        if (rol == "Estudiante" && entrega.UsuarioId != usuarioId)
            throw new Exception("No tienes permiso para ver esta entrega.");

        if (rol == "Profesor")
            await ValidarProfesorDelCursoAsync(entrega.Tarea, usuarioId, rol);

        return new EntregaDto
        {
            Id = entrega.Id,
            TareaId = entrega.TareaId,
            UsuarioId = entrega.UsuarioId,
            Estudiante = entrega.Usuario.Nombre,
            ArchivoUrl = entrega.ArchivoUrl,
            Comentario = entrega.Comentario,
            FechaEntrega = entrega.FechaEntrega,
            Calificacion = entrega.Calificacion,
        };
    }

    public async Task CalificarAsync(
        int entregaId,
        CalificarEntregaDto dto,
        int usuarioId,
        string rol)
    {
        var entrega = await _entregaRepository.ObtenerPorIdAsync(entregaId);

        if (entrega is null)
            throw new Exception("Entrega no encontrada.");

        await ValidarProfesorDelCursoAsync(entrega.Tarea, usuarioId, rol);

        if (dto.Calificacion < 0 || dto.Calificacion > entrega.Tarea.PuntajeMaximo)
            throw new Exception($"La calificación debe estar entre 0 y {entrega.Tarea.PuntajeMaximo}.");

        entrega.Calificacion = dto.Calificacion;

        await _entregaRepository.ActualizarAsync(entrega);
    }

    private async Task ValidarProfesorDelCursoAsync(Tarea tarea, int usuarioId, string rol)
    {
        if (rol == "Admin")
            return;

        var leccion = await _leccionRepository.ObtenerPorIdAsync(tarea.LeccionId);
        if (leccion is null)
            throw new Exception("Lección no encontrada.");

        var modulo = await _moduloRepository.ObtenerPorIdAsync(leccion.ModuloId);
        if (modulo is null)
            throw new Exception("Módulo no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(modulo.CursoId);
        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso sobre este curso.");
    }
}
