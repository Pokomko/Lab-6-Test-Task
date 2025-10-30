using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// ProductService.
/// Implement methods here.
/// </summary>
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> GetProductAsync()
    {
        var allProducts = _dbContext.Products;
        
        return await allProducts
            .AsNoTracking()
            .Where(p => p.Status == Enums.ProductStatus.Reserved)
            .OrderByDescending(p => p.Price)
            .FirstAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var startDate = new DateTime(2025, 1, 1);
        var quantity = 1000;

        var allProducts = _dbContext.Products;

        return await allProducts
            .AsNoTracking()
            .Where(p => p.ReceivedDate >= startDate && 
                        p.ReceivedDate < startDate.AddYears(1) &&
                        p.Quantity > quantity)
            .ToListAsync();
    }
}
