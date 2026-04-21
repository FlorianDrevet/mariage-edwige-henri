using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IGiftCategoryRepository
{
    Task<List<GiftCategory>> GetAllAsync();
    Task<GiftCategory?> GetByIdAsync(GiftCategoryId id);
    Task<GiftCategory?> GetByNameAsync(string name);
    Task AddAsync(GiftCategory category);
    Task DeleteAsync(GiftCategory category);
    Task<bool> IsCategoryUsedByGiftsAsync(string categoryName);
}
