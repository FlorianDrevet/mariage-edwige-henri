using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IAccommodationRepository
{
    void Add(Accommodation accommodation);
    Accommodation? GetById(AccommodationId id);
    List<Accommodation> GetAll();
    void Update(Accommodation accommodation);
    void Delete(Accommodation accommodation);
}
