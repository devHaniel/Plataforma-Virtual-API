using Application.DTOs.Inscripcion;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class InscripcionService
{
    private readonly IInscripcionRepository _inscripcionRepository;
    private readonly ICursoRepository _cursoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public InscripcionService(
        IInscripcionRepository inscripcionRepository,
        ICursoRepository cursoRepository,
        IUsuarioRepository usuarioRepository)
    {
        _inscripcionRepository = inscripcionRepository;
        _cursoRepository = cursoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<InscripcionDto> InscribirseAsync(int usuarioId, CrearInscripcionDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);

        if (usuario is null)
            throw new Exception("Usuario no encontrado.");

        var curso = await _cursoRepository.ObtenerPorIdAsync(dto.CursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        var yaInscrito = await _inscripcionRepository.ObtenerAsync(usuarioId, dto.CursoId);

        if (yaInscrito is not null)
            throw new Exception("Ya estás inscrito en este curso.");

        var inscripcion = new Inscripcion
        {
            UsuarioId = usuarioId,
            CursoId = dto.CursoId,
            FechaInscripcion = DateTime.Now,
        };

        await _inscripcionRepository.CrearAsync(inscripcion);

        return new InscripcionDto
        {
            Id = inscripcion.Id,
            UsuarioId = usuario.Id,
            Usuario = usuario.Nombre,
            CursoId = curso.Id,
            Curso = curso.Nombre,
            FechaInscripcion = inscripcion.FechaInscripcion,
        };
    }

    public async Task<List<InscripcionDto>> ObtenerMisCursosAsync(int usuarioId)
    {
        var inscripciones = await _inscripcionRepository.ObtenerPorUsuarioAsync(usuarioId);

        return inscripciones.Select(i => new InscripcionDto
        {
            Id = i.Id,
            UsuarioId = i.UsuarioId,
            Usuario = i.Usuario.Nombre,
            CursoId = i.CursoId,
            Curso = i.Curso.Nombre,
            FechaInscripcion = i.FechaInscripcion
        }).ToList();
    }

    public async Task<List<InscripcionDto>> ObtenerEstudiantesPorCursoAsync(
        int cursoId,
        int usuarioId,
        string rol)
    {
        var curso = await _cursoRepository.ObtenerPorIdAsync(cursoId);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para ver las inscripciones de este curso.");

        var inscripciones = await _inscripcionRepository.ObtenerPorCursoAsync(cursoId);

        return inscripciones.Select(i => new InscripcionDto
        {
            Id = i.Id,
            UsuarioId = i.UsuarioId,
            Usuario = i.Usuario.Nombre,
            CursoId = i.CursoId,
            Curso = curso.Nombre,
            FechaInscripcion = i.FechaInscripcion
        }).ToList();
    }

    public async Task CancelarInscripcionAsync(int usuarioId, int cursoId)
    {
        var inscripcion = await _inscripcionRepository.ObtenerAsync(usuarioId, cursoId);

        if (inscripcion is null)
            throw new Exception("No estás inscrito en este curso.");

        await _inscripcionRepository.EliminarAsync(inscripcion);
    }
}
