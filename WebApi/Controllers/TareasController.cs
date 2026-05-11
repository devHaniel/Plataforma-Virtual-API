using System.Security.Claims;
using Application.DTOs.Tarea;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareaService _tareaService;

        public TareasController(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [HttpGet("lecciones/{leccionId:int}/tareas")]
        [Authorize]
        public async Task<IActionResult> ObtenerPorLeccion(int leccionId)
        {
            var tareas = await _tareaService.ObtenerPorLeccionAsync(leccionId);
            return Ok(tareas);
        }

        [HttpGet("tareas/{id:int}")]
        [Authorize]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tarea = await _tareaService.ObtenerPorIdAsync(id);
            return Ok(tarea);
        }

        [HttpPost("lecciones/{leccionId:int}/tareas")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Crear(int leccionId, CrearTareaDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            var tarea = await _tareaService.CrearAsync(
                leccionId,
                dto,
                usuarioId,
                rol
            );

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = tarea.Id },
                tarea
            );
        }

        [HttpPut("tareas/{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Actualizar(int id, ActualizarTareaDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _tareaService.ActualizarAsync(
                id,
                dto,
                usuarioId,
                rol
            );

            return NoContent();
        }

        [HttpDelete("tareas/{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _tareaService.EliminarAsync(id, usuarioId, rol);

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
