namespace Mariage.Contracts.Accommodation;

public record AccommodationAssignmentResponse(
    Guid UserId,
    string Username,
    string ResponseStatus);
