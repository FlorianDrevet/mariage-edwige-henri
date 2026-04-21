using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class GiftCategory
    {
        public static Error GiftCategoryNotFound() => Error.NotFound(
            code: "GiftCategory.NotFound",
            description: "The gift category was not found."
        );

        public static Error GiftCategoryInUse() => Error.Conflict(
            code: "GiftCategory.InUse",
            description: "The gift category is still used by one or more gifts and cannot be deleted."
        );

        public static Error GiftCategoryDuplicateName() => Error.Conflict(
            code: "GiftCategory.DuplicateName",
            description: "A gift category with that name already exists."
        );
    }
}
