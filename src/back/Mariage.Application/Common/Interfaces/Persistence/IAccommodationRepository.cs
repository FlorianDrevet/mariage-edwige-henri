using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IAccommodationRepository
{
    Task<Accommodation?> GetByIdAsync(AccommodationId id);
    Task<List<Accommodation>> GetAllAsync();
    Task<List<Accommodation>> GetByUserBookingsAsync(UserId userId);
    Task AddAsync(Accommodation accommodation);
    Task UpdateAsync(Accommodation accommodation);
    Task DeleteAsync(Accommodation accommodation);
}
