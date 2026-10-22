namespace NorthWind.Membership.Entities.Validators.UserLogin;

internal class UserCredentialsDtoValidator : 
    AbstractModelValidator<UserCredentialsDto> 
{ 
    public UserCredentialsDtoValidator( 
        IValidationService<UserCredentialsDto> validationService) : 
        base(validationService) 
    { 
        AddRuleFor(u => u.Email) 
            .NotEmpty(UserLoginMessages.RequiredEmailErrorMessage) 
            .EmailAddress(UserLoginMessages.InvalidEmailErrorMessage)
            .Must(email => !string.IsNullOrWhiteSpace(email) && 
                           System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$"),
                  "El correo debe tener un dominio válido (ejemplo: usuario@dominio.com)"); 
        AddRuleFor(u => u.Password) 
            .NotEmpty(UserLoginMessages.RequiredPasswordErrorMessage); 
    } 
}
