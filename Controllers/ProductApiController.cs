using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Models;
using ProductManagementApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ProductManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductApiController> _logger;

        public ProductApiController(IProductService productService, ILogger<ProductApiController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: api/ProductApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                
                // Return with explicit property names to ensure correct casing
                var result = products.Select(p => new {
                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    price = p.Price,
                    isAvailable = p.IsAvailable,
                    createdAt = p.CreatedAt
                });
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        // GET: api/ProductApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found", id);
                    return NotFound();
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }

        // PUT: api/ProductApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var success = await _productService.UpdateProductAsync(product);
                if (!success)
                {
                    return NotFound();
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the product");
            }
        }

        // POST: api/ProductApi
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An error occurred while creating the product");
            }
        }

        // DELETE: api/ProductApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }
    }
}