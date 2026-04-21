using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mariage.Infrastructure.Persistence.Configurations;

public class GiftConfiguration: IEntityTypeConfiguration<Gift>
{
    public void Configure(EntityTypeBuilder<Gift> builder)
    {
        ConfigureGiftsTable(builder);
        ConfigureGiftGiver(builder);
    }

    private void ConfigureGiftGiver(EntityTypeBuilder<Gift> builder)
    {
        builder.OwnsMany(gift => gift.GiftGivers, gb =>
        {
            gb.ToTable("GiftGivers");
            gb.WithOwner().HasForeignKey("GiftId");
            gb.HasKey("Id", "GiftId");
            gb.Property(giftGiver => giftGiver.Id)
                .HasColumnName("GiftGiverId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => GiftGiverId.Create(value));
        });
        
        builder.Metadata.FindNavigation(nameof(Gift.GiftGivers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureGiftsTable(EntityTypeBuilder<Gift> builder)
    {
        builder.ToTable("Gifts");
        builder.HasKey(gift => gift.Id);
        builder.Property(gift => gift.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => GiftId.Create(value)
            );
        builder.Property(gift => gift.Category)
            .IsRequired()
            .HasMaxLength(100);
    }
}