using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using ProductManagementApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Configure EF Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

// Add memory cache
builder.Services.AddMemoryCache();

// Register services
builder.Services.AddScoped<IProductService, CachedProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed the database with a default user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var defaultUser = new IdentityUser
    {
        UserName = "admin@example.com",
        Email = "admin@example.com",
        EmailConfirmed = true
    };

    if (userManager.FindByEmailAsync(defaultUser.Email).Result == null)
    {
        var result = userManager.CreateAsync(defaultUser, "Admin123!").Result;
    }
}

// Seed the database with products
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Check if products already exist
    if (!context.Products.Any())
    {
        // Electronics category
        context.Products.AddRange(
            new Product
            {
                Name = "MacBook Pro 16\"",
                Description = "Apple MacBook Pro with M2 Pro chip, 16GB RAM, 512GB SSD",
                Price = 2499.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Dell XPS 15",
                Description = "Dell XPS 15 with Intel i9, 32GB RAM, 1TB SSD, NVIDIA RTX 3050 Ti",
                Price = 2199.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "iPhone 15 Pro",
                Description = "Apple iPhone 15 Pro with A17 Pro chip, 256GB storage, Titanium finish",
                Price = 1199.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Samsung Galaxy S23 Ultra",
                Description = "Samsung Galaxy S23 Ultra with Snapdragon 8 Gen 2, 12GB RAM, 512GB storage",
                Price = 1299.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Sony WH-1000XM5",
                Description = "Sony WH-1000XM5 Wireless Noise Cancelling Headphones",
                Price = 399.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "iPad Pro 12.9\"",
                Description = "Apple iPad Pro 12.9\" with M2 chip, 256GB storage, Wi-Fi + Cellular",
                Price = 1299.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "LG C2 65\" OLED TV",
                Description = "LG C2 65\" OLED evo Gallery Edition Smart TV",
                Price = 1799.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Bose QuietComfort Earbuds II",
                Description = "Bose QuietComfort Earbuds II with advanced noise cancellation",
                Price = 299.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            }
        );

        // Home & Kitchen category
        context.Products.AddRange(
            new Product
            {
                Name = "Dyson V15 Detect",
                Description = "Dyson V15 Detect cordless vacuum with laser dust detection",
                Price = 749.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "KitchenAid Stand Mixer",
                Description = "KitchenAid Artisan Series 5-Quart Tilt-Head Stand Mixer",
                Price = 399.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Ninja Foodi 10-in-1",
                Description = "Ninja Foodi 10-in-1 Smart XL Air Fry Oven",
                Price = 349.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Breville Barista Express",
                Description = "Breville Barista Express Espresso Machine",
                Price = 699.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "iRobot Roomba j7+",
                Description = "iRobot Roomba j7+ Self-Emptying Robot Vacuum",
                Price = 799.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Instant Pot Duo Plus",
                Description = "Instant Pot Duo Plus 9-in-1 Electric Pressure Cooker",
                Price = 129.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Vitamix 5200 Blender",
                Description = "Vitamix 5200 Professional-Grade Blender",
                Price = 549.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Cuisinart Food Processor",
                Description = "Cuisinart 14-Cup Food Processor",
                Price = 229.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            }
        );

        // Furniture category
        context.Products.AddRange(
            new Product
            {
                Name = "Herman Miller Aeron Chair",
                Description = "Herman Miller Aeron Ergonomic Office Chair",
                Price = 1395.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Secretlab TITAN Evo 2022",
                Description = "Secretlab TITAN Evo 2022 Gaming Chair",
                Price = 549.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "West Elm Sofa",
                Description = "West Elm Modern Sectional Sofa",
                Price = 2499.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "IKEA MALM Bed Frame",
                Description = "IKEA MALM Bed Frame with storage, Queen size",
                Price = 499.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Pottery Barn Dining Table",
                Description = "Pottery Barn Toscana Extending Dining Table",
                Price = 1899.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Article Sven Sofa",
                Description = "Article Sven Charme Tan Sofa",
                Price = 1299.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Floyd Platform Bed",
                Description = "Floyd The Platform Bed with Headboard",
                Price = 895.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "CB2 Coffee Table",
                Description = "CB2 Peekaboo Acrylic Coffee Table",
                Price = 379.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            }
        );

        // Clothing category
        context.Products.AddRange(
            new Product
            {
                Name = "Nike Air Max 270",
                Description = "Nike Air Max 270 Men's Shoes",
                Price = 150.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Levi's 501 Original Jeans",
                Description = "Levi's 501 Original Fit Men's Jeans",
                Price = 69.50m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "The North Face Jacket",
                Description = "The North Face Thermoball Eco Jacket",
                Price = 199.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Adidas Ultraboost",
                Description = "Adidas Ultraboost 22 Running Shoes",
                Price = 190.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Patagonia Better Sweater",
                Description = "Patagonia Better Sweater Fleece Jacket",
                Price = 139.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Ray-Ban Aviator Sunglasses",
                Description = "Ray-Ban Aviator Classic Sunglasses",
                Price = 161.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Herschel Supply Co. Backpack",
                Description = "Herschel Supply Co. Little America Backpack",
                Price = 109.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Timberland Boots",
                Description = "Timberland Premium 6-Inch Waterproof Boots",
                Price = 198.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            }
        );

        // Books & Media category
        context.Products.AddRange(
            new Product
            {
                Name = "Atomic Habits",
                Description = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones by James Clear",
                Price = 24.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "The Psychology of Money",
                Description = "The Psychology of Money: Timeless lessons on wealth, greed, and happiness by Morgan Housel",
                Price = 19.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Dune",
                Description = "Dune by Frank Herbert (Hardcover)",
                Price = 30.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Project Hail Mary",
                Description = "Project Hail Mary by Andy Weir",
                Price = 28.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "The Midnight Library",
                Description = "The Midnight Library by Matt Haig",
                Price = 26.00m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Kindle Paperwhite",
                Description = "Kindle Paperwhite (8 GB) with a 6.8\" display",
                Price = 139.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Audible Subscription",
                Description = "Audible Premium Plus: 1-Year Subscription",
                Price = 149.50m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            },
            new Product
            {
                Name = "Spotify Premium",
                Description = "Spotify Premium: 1-Year Subscription",
                Price = 119.88m,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            }
        );

        context.SaveChanges();
    }
}

// Seed the database with sample products
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // Check if there are any products
    if (!context.Products.Any())
    {
        // Add sample products
        context.Products.AddRange(
            new Product
            {
                Name = "MacBook Pro 16-inch",
                Description = "Apple M1 Pro chip, 16GB RAM, 512GB SSD",
                Price = 2499.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now.AddDays(-30)
            },
            new Product
            {
                Name = "iPhone 13 Pro",
                Description = "6.1-inch Super Retina XDR display, A15 Bionic chip",
                Price = 999.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now.AddDays(-15)
            },
            new Product
            {
                Name = "Samsung Galaxy S21",
                Description = "6.2-inch Dynamic AMOLED 2X, Exynos 2100",
                Price = 799.99m,
                IsAvailable = false,
                CreatedAt = DateTime.Now.AddDays(-45)
            },
            new Product
            {
                Name = "Sony WH-1000XM4",
                Description = "Wireless Noise-Canceling Headphones",
                Price = 349.99m,
                IsAvailable = true,
                CreatedAt = DateTime.Now.AddDays(-60)
            }
        );
        
        context.SaveChanges();
    }
}

app.Run();