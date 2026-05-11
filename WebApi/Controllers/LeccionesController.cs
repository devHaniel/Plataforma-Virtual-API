using System.Security.Claims;
using Application.DTOs.Leccion;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeccionesController : ControllerBase
    {
        private readonly LeccionService _leccionService;

        public LeccionesController(LeccionService leccionService)
        {
            _leccionService = leccionService;
        }

        [HttpGet("modulos/{moduloId:int}/lecciones")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorModulo(int moduloId)
        {
            var lecciones = await _leccionService.ObtenerPorModuloAsync(moduloId);
            return Ok(lecciones);
        }

        [HttpGet("lecciones/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var leccion = await _leccionService.ObtenerPorIdAsync(id);
            return Ok(leccion);
        }

        [HttpPost("modulos/{moduloId:int}/lecciones")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Crear(int moduloId, CrearLeccionDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            var leccion = await _leccionService.CrearAsync(moduloId, dto, usuarioId, rol);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = leccion.Id },
                leccion
            );
        }

        [HttpPut("lecciones/{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Actualizar(int id, ActualizarLeccionDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _leccionService.ActualizarAsync(id, dto, usuarioId, rol);

            return NoContent();
        }

        [HttpDelete("lecciones/{id:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _leccionService.EliminarAsync(id, usuarioId, rol);

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
