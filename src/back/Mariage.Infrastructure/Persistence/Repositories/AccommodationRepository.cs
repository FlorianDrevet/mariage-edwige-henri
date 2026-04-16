using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class AccommodationRepository(MariageDbContext mariageDbContext) : IAccommodationRepository
{
    public void Add(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Add(accommodation);
        mariageDbContext.SaveChanges();
    }

    public Accommodation? GetById(AccommodationId id)
    {
        return mariageDbContext.Accommodations
            .FirstOrDefault(a => a.Id == id);
    }

    public Dictionary<AccommodationId, Accommodation> GetAllById(IEnumerable<AccommodationId> ids)
    {
        var idSet = ids.ToHashSet();
        return mariageDbContext.Accommodations
            .Where(a => idSet.Contains(a.Id))
            .ToDictionary(a => a.Id);
    }

    public List<Accommodation> GetAll()
    {
        return mariageDbContext.Accommodations.ToList();
    }

    public void Update(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Update(accommodation);
        mariageDbContext.SaveChanges();
    }

    public void Delete(Accommodation accommodation)
    {
        mariageDbContext.Accommodations.Remove(accommodation);
        mariageDbContext.SaveChanges();
    }
}
