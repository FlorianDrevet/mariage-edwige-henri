using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class GiftCategoryRepository(MariageDbContext mariageDbContext) : IGiftCategoryRepository
{
    public async Task<List<GiftCategory>> GetAllAsync()
    {
        return await mariageDbContext.GiftCategories.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<GiftCategory?> GetByIdAsync(GiftCategoryId id)
    {
        return await mariageDbContext.GiftCategories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<GiftCategory?> GetByNameAsync(string name)
    {
        return await mariageDbContext.GiftCategories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task AddAsync(GiftCategory category)
    {
        mariageDbContext.GiftCategories.Add(category);
        await mariageDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(GiftCategory category)
    {
        mariageDbContext.GiftCategories.Remove(category);
        await mariageDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsCategoryUsedByGiftsAsync(string categoryName)
    {
        return await mariageDbContext.Gifts.AnyAsync(g => g.Category == categoryName);
    }
}
