namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserLogin;

public class UserLoginViewModel(
    MembershipGateway gateway,
    IModelValidatorHub<UserLoginViewModel> validator,
    JWTAuthenticationStateProvider authenticationStateProvider)
{
    public IModelValidatorHub<UserLoginViewModel> Validator => validator;
    public ModelValidator<UserLoginViewModel> ModelValidatorComponentReference
    { get; set; }

    public event Action OnLogin;
    #region Datos de entrada
    private string _email = string.Empty;
    public string Email 
    { 
        get => _email; 
        set => _email = FilterEmail(value); 
    }
    public string Password { get; set; }
    #endregion

    private static string FilterEmail(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var filtered = new string(value.Where(c => char.IsAsciiLetterOrDigit(c) || c == '.' || c == '_' || c == '-' || c == '@').ToArray());
        int atIndex = filtered.IndexOf('@');
        if (atIndex != -1)
        {
            filtered = filtered[..(atIndex + 1)] + filtered[(atIndex + 1)..].Replace("@", "");
        }
        return filtered.Length > 100 ? filtered[..100] : filtered;
    }
    public async Task Login()
    {
        try
        {
            TokensDto Tokens =
                await gateway.LoginAsync((UserCredentialsDto)this);
            await authenticationStateProvider.LoginAsync(Tokens);
            OnLogin?.Invoke();
        }
        catch (HttpRequestException ex)
        {
            if (ex.Data.Contains("Errors"))
            {
                var Errors = ex.Data["Errors"] as
                    IEnumerable<ValidationError>;
                ModelValidatorComponentReference.AddErrors(Errors);
            }
            else
            {
                throw;
            }
        }
    }
    public static explicit operator UserCredentialsDto(
        UserLoginViewModel model) =>
        new UserCredentialsDto(model.Email, model.Password);
}
