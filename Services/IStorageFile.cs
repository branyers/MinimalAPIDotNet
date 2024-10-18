

namespace MimimalAPiPeliculas.Services;

public interface IStorageFile
{
    Task DeleteFile(string? ruta, string container);
    Task<string> StorageFile(Stream file, string name, string container);

    async Task<string> Update(string route, string container, Stream file, string name)
    {
        await DeleteFile(route, container);
        return await StorageFile(file,name,container);
    }
    

}