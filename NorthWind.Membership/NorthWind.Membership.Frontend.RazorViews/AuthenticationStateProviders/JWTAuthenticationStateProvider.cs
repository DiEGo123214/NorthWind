namespace NorthWind.Membership.Frontend.RazorViews.AuthenticationStateProviders;

public class JWTAuthenticationStateProvider(ITokenStorage storage) : 
    AuthenticationStateProvider 
{ 
    public override async Task<AuthenticationState> 
        GetAuthenticationStateAsync() 
    { 
        // Un ClaimsIdentity vacío indica que no hay un usuario autenticado. 
        ClaimsIdentity Identity = new ClaimsIdentity(); 
        // Recuperar el token. 
        var Tokens = await storage.GetTokensAsync(); 
        if (Tokens != null) 
        { 
            // Se encontró el Token. Procesarlo. 
            var Handler = new JsonWebTokenHandler(); 
            var Token = Handler.ReadJsonWebToken(Tokens.AccessToken); 
            // Obtener la identidad del usuario. 
            Identity = new ClaimsIdentity( 
                Token.Claims, 
                nameof(JWTAuthenticationStateProvider)); 
        } 
        return new AuthenticationState( 
            new ClaimsPrincipal(Identity)); 
    } 
    // Almacenar el Token. 
    public async ValueTask LoginAsync(TokensDto tokens) 
    { 
        await storage.StoreTokensAsync(tokens); 
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); 
    } 
    // Eliminar el Token. 
    public async ValueTask LogoutAsync() 
    { 
        await storage.RemoveTokensAsync(); 
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); 
    } 
}
