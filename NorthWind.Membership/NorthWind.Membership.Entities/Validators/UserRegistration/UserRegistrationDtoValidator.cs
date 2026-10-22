namespace NorthWind.Membership.Entities.Validators.UserRegistration;

internal class UserRegistrationDtoValidator : 
    AbstractModelValidator<UserRegistrationDto> 
{ 
    public UserRegistrationDtoValidator( 
        IValidationService<UserRegistrationDto> validationService) : 
        base(validationService) 
    { 
        AddRuleFor(u => u.FirstName) 
            .NotEmpty(UserRegistrationMessages.RequiredFirstNameErrorMessage); 
        AddRuleFor(u => u.LastName) 
            .NotEmpty(UserRegistrationMessages.RequiredLastNameErrorMessage); 
        AddRuleFor(u => u.Email) 
            .NotEmpty(UserRegistrationMessages.RequiredEmailErrorMessage) 
            .EmailAddress(UserRegistrationMessages.InvalidEmailErrorMessage)
            .Must(email => !string.IsNullOrWhiteSpace(email) && 
                           System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$"),
                  "El correo debe tener un dominio válido (ejemplo: usuario@dominio.com)"); 
        AddRuleFor(u => u.Password) 
            .StopOnFirstFailure() 
            .NotEmpty(UserRegistrationMessages.RequiredPasswordErrorMessage) 
            .MinimumLength(6, 
                UserRegistrationMessages.PasswordTooShortErrorMessage) 
            .Must(p => p.Any(c => char.IsLower(c)), 
                UserRegistrationMessages.PasswordRequiresLowerErrorMessage) 
            .Must(p => p.Any(c => char.IsUpper(c)), 
                UserRegistrationMessages.PasswordRequiresUpperErrorMessage) 
            .Must(p => p.Any(c => char.IsDigit(c)), 
                UserRegistrationMessages.PasswordRequiresDigitErrorMessage) 
            .Must(p => p.Any(c => !char.IsLetterOrDigit(c)), 
                UserRegistrationMessages 
                .PasswordRequiresNonAlphanumericErrorMessage); 
    } 
}
