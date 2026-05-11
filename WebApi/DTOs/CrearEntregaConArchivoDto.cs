namespace Application.DTOs.Entrega;

public class CrearEntregaConArchivoDto
{
    public int TareaId { get; set; }
    public string? Comentario { get; set; }
    public IFormFile Archivo { get; set; } = null!;
}