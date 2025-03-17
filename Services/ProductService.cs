using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApp.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Use AsNoTracking for read-only operations to improve performance
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            // Prevent CreatedAt from being modified
            _context.Entry(product).Property(x => x.CreatedAt).IsModified = false;
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExistsAsync(product.Id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .SumAsync(p => p.Price);
        }

        public async Task<int> GetInStockCountAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .CountAsync(p => p.IsAvailable);
        }

        public async Task<int> GetOutOfStockCountAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .CountAsync(p => !p.IsAvailable);
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
} 