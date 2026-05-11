using System.Security.Claims;
using Application.DTOs.Entrega;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregasController : ControllerBase
    {
        private readonly EntregaService _entregaService;
        private readonly IFileService _fileService;

        public EntregasController(EntregaService entregaService, IFileService fileService)
        {
            _entregaService = entregaService;
            _fileService = fileService;
        }

        [HttpGet("{id:int}/archivo")]
        public async Task<IActionResult> DescargarArchivo(int id)
        {
            var entrega = await _entregaService.ObtenerPorIdAsync(
                id,
                ObtenerUsuarioId(),
                ObtenerRol()
            );

            var bytes = await System.IO.File.ReadAllBytesAsync(entrega.ArchivoUrl!);

            return File(bytes, "application/pdf", "entrega.pdf");
        }

        [HttpPost]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Enviar([FromForm] CrearEntregaConArchivoDto dto)
        {
            var usuarioId = ObtenerUsuarioId();

            var archivoUrl = await _fileService.GuardarArchivoAsync(
                dto.Archivo.OpenReadStream(),
                dto.Archivo.FileName,
                "entregas"
            );

            var entrega = await _entregaService.EnviarAsync(
                dto.TareaId,
                dto.Comentario,
                archivoUrl,
                usuarioId
            );

            return Ok(entrega);
        }

        [HttpGet("tarea/{tareaId:int}")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> ObtenerPorTarea(int tareaId)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            var entregas = await _entregaService.ObtenerPorTareaAsync(
                tareaId,
                usuarioId,
                rol
            );

            return Ok(entregas);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Estudiante,Profesor,Admin")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            var entrega = await _entregaService.ObtenerPorIdAsync(id, usuarioId, rol);

            return Ok(entrega);
        }

        [HttpPut("{id:int}/calificar")]
        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Calificar(int id, CalificarEntregaDto dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = ObtenerRol();

            await _entregaService.CalificarAsync(id, dto, usuarioId, rol);

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
