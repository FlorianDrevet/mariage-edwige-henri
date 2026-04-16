using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Accommodation
    {
        public static Error NotFound() => Error.NotFound(
            code: "Accommodation.NotFound",
            description: "The accommodation was not found."
        );
    }
}
