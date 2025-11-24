using ErrorOr;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Gifts.Commands.CreateGiftParticipation;

public record CreateGiftParticipationCommand(
    string FirstName,
    string LastName,
    string Email,
    float Amount,
    GiftId GiftId
    ) : IRequest<ErrorOr<Gift>>;