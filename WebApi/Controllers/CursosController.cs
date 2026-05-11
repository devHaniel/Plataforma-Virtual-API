using System.Security.Claims;
using Application.DTOs.Curso;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly CursoService _cursoService;

        public CursosController(CursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerTodos()
        {
            var cursos = await _cursoService.ObtenerTodosAsync();
            return Ok(cursos);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var curso = await _cursoService.ObtenerPorIdAsync(id);
            return Ok(curso);
        }

        [HttpPost]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Crear(CrearCursoDto dto)
        {
            var usuarioId = ObtenerUsuarioId();

            var curso = await _cursoService.CrearAsync(dto, usuarioId);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = curso.Id },
                curso
            );
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Actualizar(int id, ActualizarCursoDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _cursoService.ActualizarAsync(id, dto, usuarioId, rol);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _cursoService.EliminarAsync(id, usuarioId, rol);

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
