using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class Participation
    {
        public static Error AmountExceedParticipationLeft() => Error.Validation(
            code: "Participation.AmountExceedParticipationLeft",
            description: "The amount exceed the participation left."
        );
    }
}