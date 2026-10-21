using NorthWind.Sales.Backend.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Id)
            .HasMaxLength(10)
            .IsFixedLength();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(c => c.CurrentBalance)
            .HasPrecision(8, 2);

        builder.Property(c => c.Address)
            .HasMaxLength(60);

        builder.Property(c => c.City)
            .HasMaxLength(15);

        builder.Property(c => c.Country)
            .HasMaxLength(15);

        builder.Property(c => c.PostalCode)
            .HasMaxLength(10);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.HasData(
            new Customer
            {
                Id = "1801234567",
                Name = "Alfreds Futterkiste",
                CurrentBalance = 0,
                Address = "Obere Str. 57",
                City = "Berlin",
                Country = "Germany",
                PostalCode = "12209",
                Email = "alfreds@northwind.com"
            },
            new Customer
            {
                Id = "1712345678",
                Name = "Ana Trujillo Emparedados y helados",
                CurrentBalance = 0,
                Address = "Avda. de la Constitución 2222",
                City = "México D.F.",
                Country = "Mexico",
                PostalCode = "05021",
                Email = "ana.trujillo@northwind.com"
            },
            new Customer
            {
                Id = "0923456789",
                Name = "Antonio Moreno Taquería",
                CurrentBalance = 100,
                Address = "Mataderos 2312",
                City = "México D.F.",
                Country = "Mexico",
                PostalCode = "05023",
                Email = "antonio.moreno@northwind.com"
            });
    }
}