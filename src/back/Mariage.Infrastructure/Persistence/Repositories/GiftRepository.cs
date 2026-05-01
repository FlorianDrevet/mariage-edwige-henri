using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

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
        return mariageDbContext.Gifts.Include(g => g.GiftGivers).ToList();
    }

    public Gift? GetGiftById(GiftId requestGiftId)
    {
        return mariageDbContext.Gifts
            .Include(g => g.GiftGivers)
            .FirstOrDefault(g => g.Id == requestGiftId);
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
        // The gift entity must already be tracked by the context (loaded within the same request scope).
        // EF Core change tracking detects the modified properties and persists only the changed fields.
        mariageDbContext.SaveChanges();
    }

    public void DeleteGift(Gift gift)
    {
        mariageDbContext.Remove(gift);
        mariageDbContext.SaveChanges();
    }
}