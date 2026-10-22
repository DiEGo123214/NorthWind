using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Validators.Entities.Resources;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Validators.Entities.CreateOrder;

internal class CreateOrderDtoValidator : AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(
        IValidationService<CreateOrderDto> validationService,
        IModelValidator<CreateOrderDetailDto> detailValidator) : base(validationService)
    {
        AddRuleFor(c => c.CustomerId)
            .NotEmpty(CreateOrderMessages.CustomerIdRequired)
            .Length(10, CreateOrderMessages.CustomerIdRequiredLength)
            .Must(c => string.IsNullOrEmpty(c) || c.All(char.IsDigit), "La cédula del cliente debe contener únicamente dígitos numéricos.");

        AddRuleFor(c => c.ShipAddress)
            .NotEmpty(CreateOrderMessages.ShipAddressRequired)
            .MaximumLength(60, CreateOrderMessages.ShipAddressMaximumLength)
            .Must(a => string.IsNullOrEmpty(a) || System.Text.RegularExpressions.Regex.IsMatch(a, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s.,\-#/:°]+$"),
                  "La dirección contiene caracteres no permitidos.");

        AddRuleFor(c => c.ShipCity)
            .NotEmpty(CreateOrderMessages.ShipCityRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCityMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCityMaximumLength)
            .Must(c => string.IsNullOrEmpty(c) || c.All(ch => char.IsLetter(ch) || char.IsWhiteSpace(ch)), CreateOrderMessages.ShipCityOnlyLetters);

        AddRuleFor(c => c.ShipCountry)
            .NotEmpty(CreateOrderMessages.ShipCountryRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCountryMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCountryMaximumLength)
            .Must(c => string.IsNullOrEmpty(c) || c.All(ch => char.IsLetter(ch) || char.IsWhiteSpace(ch)), CreateOrderMessages.ShipCountryOnlyLetters);

        AddRuleFor(c => c.ShipPostalCode)
            .MaximumLength(6, "El código postal no puede exceder los 6 dígitos.")
            .Must(c => string.IsNullOrEmpty(c) || (c.All(char.IsDigit) && c.Length == 6), 
                  "El código postal debe tener exactamente 6 dígitos numéricos.");

        AddRuleFor(c => c.OrderDetails)
            .NotNull(CreateOrderMessages.OrderDetailsRequired)
            .NotEmpty(CreateOrderMessages.OrderDetailsNotEmpty);

        AddRuleForEach(c => c.OrderDetails)
            .SetValidator(detailValidator);
    }
}
