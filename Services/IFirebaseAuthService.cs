namespace MimimalAPiPeliculas.Services;

public interface IFirebaseAuthService
{
    Task<string> GetAuthTokenAsync();

}