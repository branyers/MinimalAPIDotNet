namespace MimimalAPiPeliculas.Services;

public class StorageLocalFile: IStorageFile
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _contextAccessor;


    public StorageLocalFile(IWebHostEnvironment Env, IHttpContextAccessor contextAccessor)
    {
        _webHostEnvironment = Env;
        _contextAccessor = contextAccessor;
        
    }

    public Task DeleteFile(string? ruta, string container)
    {
        if (string.IsNullOrEmpty(ruta))
        {
            return Task.CompletedTask;
        }
        
        var fileName = Path.GetFileName(ruta);
        var fileDirectory = Path.Combine(_webHostEnvironment.WebRootPath, container, fileName);
        
        if (File.Exists(fileDirectory))
        {
            File.Delete(fileDirectory);
        }
        
        return Task.CompletedTask;

    }

   public async Task<string> StorageFile(Stream file, string name, string container)
   
{
    var relativePath = Path.Combine(name);
    var extension = Path.GetExtension(relativePath);
    
    var fileName = $"{Guid.NewGuid()}{extension}";
    var folder = Path.Combine(_webHostEnvironment.WebRootPath, container);
    
    if (!Directory.Exists(folder))
    {
        Directory.CreateDirectory(folder);
    }
    
    var ruta = Path.Combine(folder, fileName);

    using (var ms = new MemoryStream())
    {
        await file.CopyToAsync(ms);
        var content = ms.ToArray();
        await File.WriteAllBytesAsync(ruta, content);
    }
    var uri = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
    var urlArchivo = Path.Combine(uri, container, fileName).Replace("\\", "/");
    return urlArchivo;
}
    
}