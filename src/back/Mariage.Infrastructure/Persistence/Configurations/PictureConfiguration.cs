using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mariage.Infrastructure.Persistence.Configurations;

public class PictureConfiguration: IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        ConfigurePicturesTable(builder);
    }

    private void ConfigurePicturesTable(EntityTypeBuilder<Picture> builder)
    {
        builder.ToTable("Pictures");
        builder.HasKey(picture => picture.Id);
        builder.Property(picture => picture.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => PictureId.Create(value)
            );
        builder.Property(picture => picture.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            );
        
        builder.Property(picture => picture.UrlImage)
            .IsRequired();
    }
}