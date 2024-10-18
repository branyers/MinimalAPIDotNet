using Firebase.Auth;
using Microsoft.Extensions.Options;

namespace MimimalAPiPeliculas.Services;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly FirebaseUserConfig _firebaseAuthService;
    private string? _firebaseToken;
    
    public FirebaseAuthService(IOptions<FirebaseUserConfig> firebaseUserConfig)
    {
        _firebaseAuthService = firebaseUserConfig.Value;
    }
    
    
    public async Task<string> GetAuthTokenAsync()
    {
        if (_firebaseToken != null) return _firebaseToken;
        
        
        var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseAuthService.ApiKey));
        var authResult = await auth.SignInWithEmailAndPasswordAsync(_firebaseAuthService.Email, _firebaseAuthService.Password);
        _firebaseToken = authResult.FirebaseToken;
        return _firebaseToken;
    }
    
    
}