using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.Common.Models;

namespace Mariage.Domain.AccommodationAggregate;

public sealed class Accommodation : AggregateRoot<AccommodationId>
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string UrlImage { get; private set; } = null!;

    private Accommodation(AccommodationId id, string title, string description, string urlImage)
        : base(id)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
    }

    public static Accommodation Create(string title, string description, string urlImage)
    {
        return new Accommodation(AccommodationId.CreateUnique(), title, description, urlImage);
    }

    public Accommodation() { }

    public void Update(string title, string description, string urlImage)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
    }
}
