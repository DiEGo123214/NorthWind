namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserRegistration;

public class UserRegistrationViewModel( 
    MembershipGateway gateway, 
    IModelValidatorHub<UserRegistrationViewModel> validator) 
{ 
    public IModelValidatorHub<UserRegistrationViewModel> Validator => 
        validator; 
    public ModelValidator<UserRegistrationViewModel> 
        ModelValidatorComponentReference 
    { get; set; } 
    public string InformationMessage { get; private set; } 
    #region Datos de entrada 
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;

    public string FirstName 
    { 
        get => _firstName; 
        set => _firstName = FilterLettersOnly(value, 30); 
    } 

    public string LastName 
    { 
        get => _lastName; 
        set => _lastName = FilterLettersOnly(value, 30); 
    } 

    private string _email = string.Empty;
    public string Email 
    { 
        get => _email; 
        set => _email = FilterEmail(value); 
    } 
    public string Password { get; set; } 
    public string PasswordConfirmation { get; set; } 
    #endregion 

    private static string FilterLettersOnly(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var filtered = new string(value.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());
        return filtered.Length > maxLength ? filtered[..maxLength] : filtered;
    }

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
    public async Task Register() 
    { 
        try 
        { 
            InformationMessage = ""; 
            await gateway.RegisterAsync((UserRegistrationDto)this); 
            InformationMessage = 
                UserRegistrationMessages.RegisteredUserMessage; 
        } 
        catch (HttpRequestException ex) 
        { 
            if (ex.Data.Contains("Errors")) 
            { 
                IEnumerable<ValidationError> Errors = 
                    ex.Data["Errors"] as IEnumerable<ValidationError>; 
                ModelValidatorComponentReference.AddErrors(Errors); 
            } 
            else 
            { 
                throw; 
            } 
        } 
    } 
    public static explicit operator UserRegistrationDto( 
        UserRegistrationViewModel model) => 
        new UserRegistrationDto( 
            model.Email, model.Password, 
            model.FirstName, model.LastName); 
}
