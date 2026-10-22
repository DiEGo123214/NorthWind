using Microsoft.EntityFrameworkCore;
using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.Repositories.Entities;
using System.Reflection;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

internal class NorthWindContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=10.0.2.2,1433;Database=NorthWindD8;User Id=sa;Password=DIEgo344*;TrustServerCertificate=True");
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Repositories.Entities.OrderDetail> OrderDetails { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
