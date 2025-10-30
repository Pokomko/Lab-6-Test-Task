using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// WarehouseService.
/// Implement methods here.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly ApplicationDbContext _dbContext;

    public WarehouseService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Warehouse> GetWarehouseAsync()
    {
        var allWarehouses = _dbContext.Warehouses;

        return await allWarehouses
            .AsNoTracking()
            .Select(w => new
            {
                Warehouse = w,
                Total = w.Products
                    .Where(p => p.Status == Enums.ProductStatus.ReadyForDistribution)
                    .Sum(p => p.Quantity * p.Price)
            })
            .OrderByDescending(w => w.Total)
            .Select(w => w.Warehouse)
            .FirstAsync();
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        var startDate = new DateTime(2025, 4, 1);
        var endDate = new DateTime(2025, 6, 30);

        var allWareHouses = _dbContext.Warehouses;

        return await allWareHouses
            .AsNoTracking()
            .Where(w => w.Products
                .Any(p => p.ReceivedDate >= startDate && p.ReceivedDate <= endDate))
            .ToListAsync();
    }
}
