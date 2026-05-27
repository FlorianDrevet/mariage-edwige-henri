using ErrorOr;
using Mariage.Domain.AccommodationAggregate.Entities;
using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.Common.Models;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.AccommodationAggregate;

public sealed class Accommodation : AggregateRoot<AccommodationId>
{
    private readonly List<AccommodationBooking> _bookings = new();

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string UrlImage { get; private set; } = null!;
    public decimal PricePerPersonPerNight { get; private set; }
    public int NumberOfNights { get; private set; }
    public int Capacity { get; private set; }

    public IReadOnlyList<AccommodationBooking> Bookings => _bookings.AsReadOnly();

    public int RemainingCapacity =>
        Capacity - _bookings
            .Where(b => b.BookingStatus != AccommodationBookingStatus.Cancelled)
            .Sum(b => b.NumberOfPersons);

    private Accommodation(
        AccommodationId id,
        string title,
        string description,
        string urlImage,
        decimal pricePerPersonPerNight,
        int numberOfNights,
        int capacity) : base(id)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
        PricePerPersonPerNight = pricePerPersonPerNight;
        NumberOfNights = numberOfNights;
        Capacity = capacity;
    }

    public static Accommodation Create(
        string title,
        string description,
        string urlImage,
        decimal pricePerPersonPerNight,
        int numberOfNights,
        int capacity)
    {
        return new Accommodation(
            AccommodationId.CreateUnique(),
            title,
            description,
            urlImage,
            pricePerPersonPerNight,
            numberOfNights,
            capacity);
    }

    public void Update(
        string title,
        string description,
        string urlImage,
        decimal pricePerPersonPerNight,
        int numberOfNights,
        int capacity)
    {
        Title = title;
        Description = description;
        UrlImage = urlImage;
        PricePerPersonPerNight = pricePerPersonPerNight;
        NumberOfNights = numberOfNights;
        Capacity = capacity;
    }

    public ErrorOr<AccommodationBooking> Book(
        UserId userId,
        int numberOfPersons,
        string bookerFirstName,
        string bookerLastName,
        string? bookerEmail)
    {
        if (numberOfPersons > RemainingCapacity)
            return Domain.Common.Errors.Errors.Accommodation.CapacityExceeded();

        var totalAmount = PricePerPersonPerNight * numberOfPersons * NumberOfNights;

        var booking = AccommodationBooking.Create(
            userId,
            numberOfPersons,
            totalAmount,
            bookerFirstName,
            bookerLastName,
            bookerEmail);

        _bookings.Add(booking);
        return booking;
    }

    public ErrorOr<AccommodationBooking> ConfirmBooking(AccommodationBookingId bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking is null)
            return Domain.Common.Errors.Errors.Accommodation.BookingNotFound();

        if (booking.BookingStatus == AccommodationBookingStatus.Confirmed)
            return Domain.Common.Errors.Errors.Accommodation.BookingAlreadyConfirmed();

        booking.Confirm();
        return booking;
    }

    public ErrorOr<AccommodationBooking> CancelBooking(AccommodationBookingId bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking is null)
            return Domain.Common.Errors.Errors.Accommodation.BookingNotFound();

        booking.Cancel();
        return booking;
    }

    public Accommodation() { }
}
