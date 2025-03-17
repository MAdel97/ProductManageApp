using ProductManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagementApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<decimal> GetTotalInventoryValueAsync();
        Task<int> GetInStockCountAsync();
        Task<int> GetOutOfStockCountAsync();
    }
} 