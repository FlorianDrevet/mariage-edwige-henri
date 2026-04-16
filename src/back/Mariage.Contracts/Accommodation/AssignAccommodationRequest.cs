namespace Mariage.Contracts.Accommodation;

public record AssignAccommodationRequest(
    Guid UserId,
    Guid AccommodationId);
