using Mariage.Domain.AccommodationAggregate.Entities;
using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.Common.Models;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.AccommodationAggregate;

public sealed class Accommodation : AggregateRoot<AccommodationId>
{
    private readonly List<AccommodationAssignment> _assignments = new();

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string UrlImage { get; private set; } = null!;
    public decimal Price { get; private set; }

    public IReadOnlyList<AccommodationAssignment> Assignments => _assignments.AsReadOnly();

    private Accommodation(
        AccommodationId id,
        string title,
        string description,
        string urlImage,
        decimal price) : base(id)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
        Price = price;
    }

    public static Accommodation Create(string title, string description, string urlImage, decimal price)
    {
        return new Accommodation(AccommodationId.CreateUnique(), title, description, urlImage, price);
    }

    public void Update(string title, string description, string urlImage, decimal price)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
        Price = price;
    }

    public void AssignUser(UserId userId)
    {
        if (_assignments.Any(a => a.UserId == userId))
            return;

        _assignments.Add(AccommodationAssignment.Create(userId));
    }

    public void UnassignUser(UserId userId)
    {
        var assignment = _assignments.FirstOrDefault(a => a.UserId == userId);
        if (assignment is not null)
            _assignments.Remove(assignment);
    }

    public void SetUserResponse(UserId userId, AccommodationResponseStatus status)
    {
        var assignment = _assignments.FirstOrDefault(a => a.UserId == userId);
        assignment?.Respond(status);
    }

    public Accommodation() { }
}
