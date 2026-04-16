using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class GiftRepository(MariageDbContext mariageDbContext): IGiftRepository
{
    public void AddGift(Gift gift)
    {
        mariageDbContext.Add(gift);
        mariageDbContext.SaveChanges();
    }

    public ErrorOr<List<Gift>> GetGifts()
    {
        return mariageDbContext.Gifts.ToList();
    }

    public Gift? GetGiftById(GiftId requestGiftId)
    {
        return mariageDbContext.Gifts.FirstOrDefault(g => g.Id == requestGiftId);
    }

    public Gift AddGiftGiver(GiftId giftId, GiftGiver giftGiver)
    {
        var gift = mariageDbContext.Gifts.FirstOrDefault(g => g.Id == giftId);
        gift?.AddGiftGiver(giftGiver);
        mariageDbContext.SaveChanges();
        return gift!;
    }

    public void UpdateGift(Gift gift)
    {
        mariageDbContext.Update(gift);
        mariageDbContext.SaveChanges();
    }

    public void DeleteGift(Gift gift)
    {
        mariageDbContext.Remove(gift);
        mariageDbContext.SaveChanges();
    }
}