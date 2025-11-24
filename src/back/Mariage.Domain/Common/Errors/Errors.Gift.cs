using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Gift
    {
        public static Error GiftNotFound() => Error.NotFound(
            code: "Gift.NotFound",
            description: "The gift was not found."
        );
    }
}