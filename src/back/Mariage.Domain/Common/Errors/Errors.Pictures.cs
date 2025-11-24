using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Pictures
    {
        public static Error NotFoundPictureWithIdError() => Error.NotFound(
            code: "Pictures.NotFoundPictureWithId",
            description: "A picture with the given id does not exist."
        );
    }
}