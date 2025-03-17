using Microsoft.Extensions.Caching.Memory;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagementApp.Services
{
    public class CachedProductService : IProductService
    {
        private readonly ProductService _productService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedProductService> _logger;
        private const string AllProductsCacheKey = "AllProducts";
        private const string ProductCacheKeyPrefix = "Product_";
        private const string StatsCacheKey = "ProductStats";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        public CachedProductService(
            ApplicationDbContext context, 
            IMemoryCache cache,
            ILogger<CachedProductService> logger)
        {
            _productService = new ProductService(context);
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            if (!_cache.TryGetValue(AllProductsCacheKey, out IEnumerable<Product> products))
            {
                _logger.LogInformation("Cache miss for all products");
                products = await _productService.GetAllProductsAsync();
                _cache.Set(AllProductsCacheKey, products, _cacheDuration);
            }
            else
            {
                _logger.LogInformation("Cache hit for all products");
            }
            
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            string cacheKey = $"{ProductCacheKeyPrefix}{id}";
            
            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                _logger.LogInformation("Cache miss for product {Id}", id);
                product = await _productService.GetProductByIdAsync(id);
                
                if (product != null)
                {
                    _cache.Set(cacheKey, product, _cacheDuration);
                }
            }
            else
            {
                _logger.LogInformation("Cache hit for product {Id}", id);
            }
            
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var result = await _productService.CreateProductAsync(product);
            InvalidateCache();
            return result;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var result = await _productService.UpdateProductAsync(product);
            InvalidateCache();
            return result;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            InvalidateCache();
            return result;
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            if (!_cache.TryGetValue($"{StatsCacheKey}_Value", out decimal value))
            {
                value = await _productService.GetTotalInventoryValueAsync();
                _cache.Set($"{StatsCacheKey}_Value", value, _cacheDuration);
            }
            
            return value;
        }

        public async Task<int> GetInStockCountAsync()
        {
            if (!_cache.TryGetValue($"{StatsCacheKey}_InStock", out int count))
            {
                count = await _productService.GetInStockCountAsync();
                _cache.Set($"{StatsCacheKey}_InStock", count, _cacheDuration);
            }
            
            return count;
        }

        public async Task<int> GetOutOfStockCountAsync()
        {
            if (!_cache.TryGetValue($"{StatsCacheKey}_OutOfStock", out int count))
            {
                count = await _productService.GetOutOfStockCountAsync();
                _cache.Set($"{StatsCacheKey}_OutOfStock", count, _cacheDuration);
            }
            
            return count;
        }

        private void InvalidateCache()
        {
            _cache.Remove(AllProductsCacheKey);
            _cache.Remove($"{StatsCacheKey}_Value");
            _cache.Remove($"{StatsCacheKey}_InStock");
            _cache.Remove($"{StatsCacheKey}_OutOfStock");
        }
    }
} 