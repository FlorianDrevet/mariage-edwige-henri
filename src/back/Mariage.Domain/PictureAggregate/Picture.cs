using Mariage.Domain.Common.Models;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.PictureAggregate;

public sealed class Picture : AggregateRoot<PictureId>
{
    public UserId UserId { get; private set; } = null!;
    public string UrlImage { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    
    
    private Picture(PictureId pictureId, string urlImage, UserId userId, DateTime createdAt)
        : base(pictureId)
    {
        UrlImage = urlImage;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public static Picture Create(string urlImage, UserId userId)
    {
        return new Picture(PictureId.CreateUnique(), urlImage, userId, DateTime.Now);
    }
    
    public Picture(){}
} 