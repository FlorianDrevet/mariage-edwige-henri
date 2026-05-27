using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class AccommodationRepository(MariageDbContext mariageDbContext) : IAccommodationRepository
{
    public async Task<Accommodation?> GetByIdAsync(AccommodationId id)
    {
        return await mariageDbContext.Accommodations
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Accommodation>> GetAllAsync()
    {
        return await mariageDbContext.Accommodations.ToListAsync();
    }

    public async Task<List<Accommodation>> GetByUserBookingsAsync(UserId userId)
    {
        return await mariageDbContext.Accommodations
            .Where(a => a.Bookings.Any(b => b.UserId == userId))
            .ToListAsync();
    }

    public async Task AddAsync(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Add(accommodation);
        await mariageDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Update(accommodation);
        await mariageDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Remove(accommodation);
        await mariageDbContext.SaveChangesAsync();
    }
}
