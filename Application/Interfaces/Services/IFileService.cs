namespace Application.Interfaces.Services;

public interface IFileService
{
    Task<string> GuardarArchivoAsync(
        Stream archivo,
        string nombreOriginal,
        string carpeta
    );
}