using Application.DTOs.Curso;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class CursoService
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public CursoService(
        ICursoRepository cursoRepository,
        IUsuarioRepository usuarioRepository)
    {
        _cursoRepository = cursoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<CursoDto>> ObtenerTodosAsync()
    {
        var cursos = await _cursoRepository.ObtenerTodosAsync();

        return cursos.Select(c => new CursoDto
        {
            Id = c.Id,
            Titulo = c.Nombre,
            Descripcion = c.Descripcion,
            ImagenUrl = c.ImagenUrl,
            Activo = c.Activo,
            FechaCreacion = c.FechaCreacion,
            ProfesorId = c.ProfesorId,
            Profesor = c.Profesor.Nombre
        }).ToList();
    }

    public async Task<CursoDto> ObtenerPorIdAsync(int id)
    {
        var curso = await _cursoRepository.ObtenerPorIdAsync(id);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        return new CursoDto
        {
            Id = curso.Id,
            Titulo = curso.Nombre,
            Descripcion = curso.Descripcion,
            ImagenUrl = curso.ImagenUrl,
            Activo = curso.Activo,
            FechaCreacion = curso.FechaCreacion,
            ProfesorId = curso.ProfesorId,
            Profesor = curso.Profesor.Nombre
        };
    }

    public async Task<CursoDto> CrearAsync(CrearCursoDto dto, int profesorId)
    {
        var profesor = await _usuarioRepository.ObtenerPorIdAsync(profesorId);

        if (profesor is null)
            throw new Exception("Profesor no encontrado.");

        var curso = new Curso
        {
            Nombre = dto.Titulo,
            Descripcion = dto.Descripcion,
            ImagenUrl = dto.ImagenUrl,
            ProfesorId = profesorId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _cursoRepository.CrearAsync(curso);

        curso.Profesor = profesor;

        return new CursoDto
        {
            Id = curso.Id,
            Titulo = curso.Nombre,
            Descripcion = curso.Descripcion,
            ImagenUrl = curso.ImagenUrl,
            Activo = curso.Activo,
            FechaCreacion = curso.FechaCreacion,
            ProfesorId = curso.ProfesorId,
            Profesor = profesor.Nombre
        };
    }

    public async Task ActualizarAsync(int id, ActualizarCursoDto dto, int usuarioId, string rol)
    {
        var curso = await _cursoRepository.ObtenerPorIdAsync(id);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para actualizar este curso.");

        curso.Nombre = dto.Titulo;
        curso.Descripcion = dto.Descripcion;
        curso.ImagenUrl = dto.ImagenUrl;
        curso.Activo = dto.Activo;

        await _cursoRepository.ActualizarAsync(curso);
    }

    public async Task EliminarAsync(int id, int usuarioId, string rol)
    {
        var curso = await _cursoRepository.ObtenerPorIdAsync(id);

        if (curso is null)
            throw new Exception("Curso no encontrado.");

        if (rol != "Admin" && curso.ProfesorId != usuarioId)
            throw new Exception("No tienes permiso para eliminar este curso.");

        await _cursoRepository.EliminarAsync(curso);
    }
}