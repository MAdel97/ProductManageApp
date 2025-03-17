using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Services;
using System;
using System.Threading.Tasks;

namespace ProductManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Change to [Authorize] after testing
    public class DashboardApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<DashboardApiController> _logger;

        public DashboardApiController(IProductService productService, ILogger<DashboardApiController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                var totalProducts = products.Count();
                var inStock = await _productService.GetInStockCountAsync();
                var totalValue = await _productService.GetTotalInventoryValueAsync();

                return Ok(new
                {
                    totalProducts,
                    inStock,
                    outOfStock = totalProducts - inStock,
                    totalValue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard statistics");
                return StatusCode(500, "An error occurred while retrieving dashboard statistics");
            }
        }
    }
} 