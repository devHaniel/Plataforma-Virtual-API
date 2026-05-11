using System.Security.Claims;
using Application.DTOs.Inscripcion;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
         private readonly InscripcionService _inscripcionService;

    public InscripcionesController(InscripcionService inscripcionService)
    {
        _inscripcionService = inscripcionService;
    }

    [HttpPost]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Inscribirse(CrearInscripcionDto dto)
    {
        var usuarioId = ObtenerUsuarioId();

        var inscripcion = await _inscripcionService.InscribirseAsync(usuarioId, dto);

        return Ok(inscripcion);
    }

    [HttpGet("mis-cursos")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> ObtenerMisCursos()
    {
        var usuarioId = ObtenerUsuarioId();

        var cursos = await _inscripcionService.ObtenerMisCursosAsync(usuarioId);

        return Ok(cursos);
    }

    [HttpGet("curso/{cursoId:int}")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> ObtenerEstudiantesPorCurso(int cursoId)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        var estudiantes = await _inscripcionService.ObtenerEstudiantesPorCursoAsync(
            cursoId,
            usuarioId,
            rol
        );

        return Ok(estudiantes);
    }

    [HttpDelete("curso/{cursoId:int}")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> CancelarInscripcion(int cursoId)
    {
        var usuarioId = ObtenerUsuarioId();

        await _inscripcionService.CancelarInscripcionAsync(usuarioId, cursoId);

        return NoContent();
    }

    private int ObtenerUsuarioId()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id is null)
            throw new Exception("Token inválido.");

        return int.Parse(id);
    }

    private string ObtenerRol()
    {
        var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        if (rol is null)
            throw new Exception("Rol no encontrado en el token.");

        return rol;
    }
    }
}
