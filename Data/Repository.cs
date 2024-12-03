using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TestSistemi.Models;
using System.Windows;

namespace TestSistemi.Data
{
   

    public class Repository
    {
        private readonly ProductContext _context;

        public Repository()
        {
            _context = new ProductContext();
        }

        public List<Product> GetAllProducts()
        {
            // Verifica se il contesto è null
            if (_context == null)
            {
                // Logga l'errore o restituisci una lista vuota
                Console.WriteLine("Il contesto del database è null.");
                MessageBox.Show("Errore: il contesto del database non è disponibile.",
                                "Errore",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return new List<Product>();
            }

            // Verifica se ci sono dati nella tabella
            if (!_context.Products.Any())
            {
                Console.WriteLine("Nessun prodotto trovato nel database.");
                MessageBox.Show("Non ci sono prodotti disponibili al momento.",
                                "Informazione",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return new List<Product>();
            }

            // Se tutto è in ordine, restituisci i prodotti
            return _context.Products.ToList();
        }


        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.ProductID);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Price = product.Price;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }

}
