using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal CurrentBalance { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Email { get; set; }
}