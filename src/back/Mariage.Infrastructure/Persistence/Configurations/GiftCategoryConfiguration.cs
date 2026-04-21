using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mariage.Infrastructure.Persistence.Configurations;

public class GiftCategoryConfiguration : IEntityTypeConfiguration<GiftCategory>
{
    public void Configure(EntityTypeBuilder<GiftCategory> builder)
    {
        builder.ToTable("GiftCategories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => GiftCategoryId.Create(value)
            );
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}
