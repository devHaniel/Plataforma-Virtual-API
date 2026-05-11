using Application.Interfaces.Services;

namespace Infrastructure.Services;

public class FileService : IFileService
{
    public async Task<string> GuardarArchivoAsync(
        Stream archivo,
        string nombreOriginal,
        string carpeta)
    {
        var extension = Path.GetExtension(nombreOriginal);
        var nombreGuardado = $"{Guid.NewGuid()}{extension}";

        var carpetaDestino = Path.Combine("uploads", carpeta);

        Directory.CreateDirectory(carpetaDestino);

        var rutaCompleta = Path.Combine(carpetaDestino, nombreGuardado);

        using var fileStream = new FileStream(rutaCompleta, FileMode.Create);

        await archivo.CopyToAsync(fileStream);

        return rutaCompleta;
    }
}