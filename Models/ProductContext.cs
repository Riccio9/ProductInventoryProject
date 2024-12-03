using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Windows;
using TestSistemi.Models;

namespace TestSistemi.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Stringa di connessione
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=[databasename];Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
