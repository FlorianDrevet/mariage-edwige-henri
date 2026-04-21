using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Accommodation
    {
        public static Error NotFound() => Error.NotFound(
            code: "Accommodation.NotFound",
            description: "The accommodation was not found.");

        public static Error UserAlreadyAssigned() => Error.Conflict(
            code: "Accommodation.UserAlreadyAssigned",
            description: "This user is already assigned to this accommodation.");

        public static Error AlreadyAssignedElsewhere() => Error.Conflict(
            code: "Accommodation.AlreadyAssignedElsewhere",
            description: "This user is already assigned to another accommodation.");

        public static Error UserNotAssigned() => Error.NotFound(
            code: "Accommodation.UserNotAssigned",
            description: "This user is not assigned to any accommodation.");
    }
}
