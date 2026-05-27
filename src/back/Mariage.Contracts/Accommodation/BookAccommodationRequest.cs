namespace Mariage.Contracts.Accommodation;

public record BookAccommodationRequest(
    int NumberOfPersons,
    string FirstName,
    string LastName,
    string? Email);
