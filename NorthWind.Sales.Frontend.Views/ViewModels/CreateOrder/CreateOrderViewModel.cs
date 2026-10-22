using NorthWind.RazorComponents.Validators;
using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.Views.Resources;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderViewModel(
    ICreateOrderGateway gateway,
    IModelValidatorHub<CreateOrderViewModel> validator)
{
    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;

    public ModelValidator<CreateOrderViewModel> ModelValidatorComponentReference { get; set; }

    #region Propiedades relacionadas a CreateOrderDto
    private string _shipAddress = string.Empty;
    private string _shipCity = string.Empty;
    private string _shipCountry = string.Empty;
    private string _shipPostalCode = string.Empty;

    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    public CustomerDto? SelectedCustomer { get; private set; }

    public string CustomerAddress => SelectedCustomer?.Address ?? string.Empty;
    public string CustomerCity => SelectedCustomer?.City ?? string.Empty;
    public string CustomerCountry => SelectedCustomer?.Country ?? string.Empty;
    public string CustomerPostalCode => SelectedCustomer?.PostalCode ?? string.Empty;
    public string CustomerEmail => SelectedCustomer?.Email ?? string.Empty;

    public ShippingOptionType ShippingOption { get; private set; } = ShippingOptionType.None;
    public bool IsCustomShippingAddress => ShippingOption == ShippingOptionType.CustomAddress;
    public bool IsCustomerShippingAddress => ShippingOption == ShippingOptionType.CustomerAddress;

    public string ShipAddress
    {
        get => _shipAddress;
        set => _shipAddress = FilterAddress(value, 60);
    }

    public string ShipCity
    {
        get => _shipCity;
        set => _shipCity = FilterLettersOnly(value, 15);
    }

    public string ShipCountry
    {
        get => _shipCountry;
        set => _shipCountry = FilterLettersOnly(value, 15);
    }

    public string ShipPostalCode
    {
        get => _shipPostalCode;
        set => _shipPostalCode = FilterPostalCode(value, 6);
    }

    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    #endregion

    #region Totals and Calculations
    public decimal Subtotal => Math.Round(OrderDetails.Sum(d => d.Subtotal), 2);
    public decimal TaxRate { get; set; } = 0.15m; // 15% IVA
    public decimal Tax => Math.Round(Subtotal * TaxRate, 2);
    public decimal Total => Math.Round(Subtotal + Tax, 2);
    #endregion

    public string InformationMessage { get; private set; } = string.Empty;
    public string? ErrorMessage { get; private set; }
    public int? LastCreatedOrderId { get; private set; }
    public bool HasAnyStockErrors => OrderDetails.Any(d => d.HasStockError);

    public void SelectCustomer(CustomerDto customer)
    {
        SelectedCustomer = customer;
        CustomerId = customer.Id;
        CustomerName = customer.Name;
        // Al seleccionar al cliente NO se llenan automáticamente los campos de envío.
        // Se deben llenar una vez oprimido "Usar mi dirección".
        ShippingOption = ShippingOptionType.None;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipCountry = string.Empty;
        ShipPostalCode = string.Empty;
    }

    public void ClearCustomer()
    {
        SelectedCustomer = null;
        CustomerId = string.Empty;
        CustomerName = string.Empty;
        ShippingOption = ShippingOptionType.None;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipCountry = string.Empty;
        ShipPostalCode = string.Empty;
    }

    public void ResetOrder()
    {
        OrderDetails.Clear();
        ClearCustomer();
        InformationMessage = string.Empty;
        ErrorMessage = string.Empty;
    }

    public void ClearMessages()
    {
        InformationMessage = string.Empty;
        ErrorMessage = string.Empty;
    }

    public void SelectShippingOption(ShippingOptionType option)
    {
        ShippingOption = option;
        if (option == ShippingOptionType.CustomerAddress)
        {
            // [ Usar mi dirección ]: Se llenan automáticamente con los datos registrados del cliente en la BD
            ShipAddress = SelectedCustomer?.Address ?? string.Empty;
            ShipCity = SelectedCustomer?.City ?? string.Empty;
            ShipCountry = SelectedCustomer?.Country ?? string.Empty;
            ShipPostalCode = SelectedCustomer?.PostalCode ?? string.Empty;
        }
        else if (option == ShippingOptionType.CustomAddress)
        {
            // [ Otra dirección ]: Habilita y limpia los campos de envío para escribir manualmente
            ShipAddress = string.Empty;
            ShipCity = string.Empty;
            ShipCountry = string.Empty;
            ShipPostalCode = string.Empty;
        }
        else
        {
            ShipAddress = string.Empty;
            ShipCity = string.Empty;
            ShipCountry = string.Empty;
            ShipPostalCode = string.Empty;
        }
    }

    public void UpdateSelectedCustomerLocation(CustomerDto updatedCustomer)
    {
        SelectedCustomer = updatedCustomer;
        if (ShippingOption == ShippingOptionType.CustomerAddress)
        {
            ShipAddress = updatedCustomer.Address ?? string.Empty;
            ShipCity = updatedCustomer.City ?? string.Empty;
            ShipCountry = updatedCustomer.Country ?? string.Empty;
            ShipPostalCode = updatedCustomer.PostalCode ?? string.Empty;
        }
    }

    public void AddOrUpdateProduct(ProductDto product, CreateOrderDetailViewModel? targetItem = null)
    {
        if (targetItem != null)
        {
            // Requisito 11: Actualizar o cambiar producto sin perder la fila
            targetItem.ProductId = product.Id;
            targetItem.ProductName = product.Name;
            targetItem.UnitPrice = product.UnitPrice;
            targetItem.UnitsInStock = product.UnitsInStock;
            if (targetItem.Quantity > product.UnitsInStock)
            {
                targetItem.Quantity = product.UnitsInStock;
            }
        }
        else
        {
            // Requisito 3: No duplicar producto en la orden
            var existing = OrderDetails.FirstOrDefault(d => d.ProductId == product.Id);
            if (existing != null)
            {
                if (existing.Quantity < product.UnitsInStock)
                {
                    existing.Quantity++;
                }
            }
            else
            {
                OrderDetails.Add(new CreateOrderDetailViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    Quantity = 1
                });
            }
        }
    }

    public void AddNewOrderDetailItem()
    {
        OrderDetails.Add(new CreateOrderDetailViewModel());
    }

    public void RemoveOrderDetailItem(CreateOrderDetailViewModel item)
    {
        OrderDetails.Remove(item);
    }

    public async Task Send()
    {
        InformationMessage = "";
        ErrorMessage = "";
        LastCreatedOrderId = null;

        if (HasAnyStockErrors)
        {
            ErrorMessage = "La transacción no puede completarse: uno o más productos superan el stock disponible.";
            return;
        }

        try
        {
            var OrderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            LastCreatedOrderId = OrderId;
            InformationMessage = $"¡Transacción completada con éxito! Se ha creado la orden #{OrderId} y se ha descontado el stock de los productos.";

            // Limpiar datos de la orden tras el guardado exitoso
            OrderDetails.Clear();
            ClearCustomer();
        }
        catch (HttpRequestException ex)
        {
            if (ex.Data.Contains("Errors"))
            {
                IEnumerable<ValidationError>? Errors =
                    ex.Data["Errors"] as IEnumerable<ValidationError>;
                if (Errors != null)
                {
                    ModelValidatorComponentReference?.AddErrors(Errors);
                }
                ErrorMessage = "La transacción fue rechazada debido a errores de validación. Por favor verifique los campos marcados.";
            }
            else
            {
                ErrorMessage = $"La transacción falló al comunicarse con el servidor: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error en la transacción: {ex.Message}";
        }
    }

    public static explicit operator CreateOrderDto(CreateOrderViewModel model) =>
        new CreateOrderDto(
            model.CustomerId, model.ShipAddress, model.ShipCity,
            model.ShipCountry, model.ShipPostalCode,
            model.OrderDetails.Select(d => new CreateOrderDetailDto(
                d.ProductId, d.UnitPrice, d.Quantity))
        );

    #region Métodos de Limpieza y Restricción de Entradas
    private static string FilterAddress(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        char[] allowed = ['.', ',', '-', '#', '/', ':', '°'];
        var filtered = new string(value.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || allowed.Contains(c)).ToArray());
        return filtered.Length > maxLength ? filtered[..maxLength] : filtered;
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length > maxLength ? value[..maxLength] : value;
    }

    private static string FilterLettersOnly(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var filtered = new string(value.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());
        return filtered.Length > maxLength ? filtered[..maxLength] : filtered;
    }

    private static string FilterPostalCode(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var filtered = new string(value.Where(char.IsDigit).ToArray());
        return filtered.Length > maxLength ? filtered[..maxLength] : filtered;
    }
    #endregion
}
