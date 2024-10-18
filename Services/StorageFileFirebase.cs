using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Options;

namespace MimimalAPiPeliculas.Services;

public class StorageFileFirebase: IStorageFile
{
    private readonly FirebaseUserConfig _firebaseUserConfig;

    public StorageFileFirebase(IOptions<FirebaseUserConfig> firebaseUserConfig)
    {
        _firebaseUserConfig = firebaseUserConfig.Value;
    }

    public async Task<string> StorageFile(Stream file, string name, string container)
    {

        var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseUserConfig.ApiKey));
        var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseUserConfig.Email, _firebaseUserConfig.Password);

        var cancellation = new CancellationTokenSource();
        var task = new FirebaseStorage(
                _firebaseUserConfig.StorageUrl,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
            .Child(container)
            .Child(name)
            .PutAsync(file, cancellation.Token);

        var downloadUrl = await task;
        return downloadUrl;
    }
    
    
    public async Task DeleteFile(string? ruta, string container)
    {
        if (ruta != null)
        {
            var fileName = ExtractFileName(ruta);
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseUserConfig.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseUserConfig.Email, _firebaseUserConfig.Password);
            
            await new FirebaseStorage(
                    _firebaseUserConfig.StorageUrl,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                .Child(container)
                .Child(fileName)
                .DeleteAsync();
        }
    }
    
    
    private string ExtractFileName(string ruta)
    {
        var cleanRuta = ruta.Replace("actors%2F", "");
        if (!Uri.IsWellFormedUriString(cleanRuta, UriKind.Absolute)) return Path.GetFileName(cleanRuta);
        var uri = new Uri(cleanRuta);
        return Path.GetFileName(uri.AbsolutePath);
    }
}