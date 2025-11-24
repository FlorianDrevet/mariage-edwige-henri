using ErrorOr;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IGiftRepository
{
    void AddGift(Gift gift);
    ErrorOr<List<Gift>> GetGifts();
    Gift? GetGiftById(GiftId requestGiftId);
    Gift AddGiftGiver(GiftId giftId, GiftGiver giftGiver);
}