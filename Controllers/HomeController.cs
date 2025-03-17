using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using System.Diagnostics;

namespace ProductManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var productCount = await _context.Products.CountAsync();
            var availableProductCount = await _context.Products.CountAsync(p => p.IsAvailable);
            var totalValue = await _context.Products.SumAsync(p => p.Price);

            ViewBag.ProductCount = productCount;
            ViewBag.AvailableProductCount = availableProductCount;
            ViewBag.TotalValue = totalValue;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}