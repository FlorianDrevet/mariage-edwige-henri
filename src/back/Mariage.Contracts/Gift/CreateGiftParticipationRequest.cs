namespace Mariage.Contracts.Gift;

public record CreateGiftParticipationRequest(
    string FirstName,
    string LastName,
    string Email,
    float Amount
    );