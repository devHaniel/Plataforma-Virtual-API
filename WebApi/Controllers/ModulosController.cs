using System.Security.Claims;
using Application.DTOs.Modulo;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulosController : ControllerBase
    {
        private readonly ModuloService _moduloService;

    public ModulosController(ModuloService moduloService)
    {
        _moduloService = moduloService;
    }

    [HttpGet("cursos/{cursoId:int}/modulos")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorCurso(int cursoId)
    {
        var modulos = await _moduloService.ObtenerPorCursoAsync(cursoId);
        return Ok(modulos);
    }

    [HttpGet("modulos/{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var modulo = await _moduloService.ObtenerPorIdAsync(id);
        return Ok(modulo);
    }

    [HttpPost("cursos/{cursoId:int}/modulos")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> Crear(int cursoId, CrearModuloDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        var modulo = await _moduloService.CrearAsync(
            cursoId,
            dto,
            usuarioId,
            rol
        );

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = modulo.Id },
            modulo
        );
    }

    [HttpPut("modulos/{id:int}")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> Actualizar(int id, ActualizarModuloDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        await _moduloService.ActualizarAsync(id, dto, usuarioId, rol);

        return NoContent();
    }

    [HttpDelete("modulos/{id:int}")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuarioId = ObtenerUsuarioId();
        var rol = ObtenerRol();

        await _moduloService.EliminarAsync(id, usuarioId, rol);

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
