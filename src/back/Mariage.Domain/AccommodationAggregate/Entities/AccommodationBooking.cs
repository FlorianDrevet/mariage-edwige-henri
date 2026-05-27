using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.Common.Models;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.AccommodationAggregate.Entities;

public sealed class AccommodationBooking : Entity<AccommodationBookingId>
{
    public UserId UserId { get; private set; } = null!;
    public int NumberOfPersons { get; private set; }
    public decimal TotalAmount { get; private set; }
    public AccommodationBookingStatus BookingStatus { get; private set; }
    public string BookerFirstName { get; private set; } = null!;
    public string BookerLastName { get; private set; } = null!;
    public string? BookerEmail { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AccommodationBooking(
        AccommodationBookingId id,
        UserId userId,
        int numberOfPersons,
        decimal totalAmount,
        string bookerFirstName,
        string bookerLastName,
        string? bookerEmail) : base(id)
    {
        UserId = userId;
        NumberOfPersons = numberOfPersons;
        TotalAmount = totalAmount;
        BookingStatus = AccommodationBookingStatus.Pending;
        BookerFirstName = bookerFirstName;
        BookerLastName = bookerLastName;
        BookerEmail = bookerEmail;
        CreatedAt = DateTime.UtcNow;
    }

    public static AccommodationBooking Create(
        UserId userId,
        int numberOfPersons,
        decimal totalAmount,
        string bookerFirstName,
        string bookerLastName,
        string? bookerEmail)
    {
        return new AccommodationBooking(
            AccommodationBookingId.CreateUnique(),
            userId,
            numberOfPersons,
            totalAmount,
            bookerFirstName,
            bookerLastName,
            bookerEmail);
    }

    public void Confirm()
    {
        BookingStatus = AccommodationBookingStatus.Confirmed;
    }

    public void Cancel()
    {
        BookingStatus = AccommodationBookingStatus.Cancelled;
    }

    public AccommodationBooking() { }
}
