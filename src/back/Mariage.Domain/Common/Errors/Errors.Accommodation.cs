using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Accommodation
    {
        public static Error NotFound() => Error.NotFound(
            code: "Accommodation.NotFound",
            description: "The accommodation was not found.");

        public static Error CapacityExceeded() => Error.Conflict(
            code: "Accommodation.CapacityExceeded",
            description: "The requested number of persons exceeds the remaining capacity.");

        public static Error BookingNotFound() => Error.NotFound(
            code: "Accommodation.BookingNotFound",
            description: "The booking was not found.");

        public static Error BookingAlreadyConfirmed() => Error.Conflict(
            code: "Accommodation.BookingAlreadyConfirmed",
            description: "This booking is already confirmed.");

        public static Error UserAlreadyBooked() => Error.Conflict(
            code: "Accommodation.UserAlreadyBooked",
            description: "This user already has a booking for this accommodation.");
    }
}
