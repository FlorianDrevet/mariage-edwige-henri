using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mariage.Infrastructure.Persistence.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureGuest(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            );
        
        builder.Property(picture => picture.PictureIds)
            .HasConversion(
                ids => string.Join(',', ids.Select(id => id.Value)),
                value => value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select( id => PictureId.Create(Guid.Parse(id))).ToList()
            );

        builder.Property(user => user.AccommodationId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? AccommodationId.Create(value.Value) : null
            )
            .IsRequired(false);

        builder.Property(user => user.IsAccommodationAccepted)
            .IsRequired(false);
    }
    
    private void ConfigureGuest(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(gift => gift.Guests, gb =>
        {
            gb.ToTable("Guests");
            gb.WithOwner().HasForeignKey("UserId");
            gb.HasKey("Id", "UserId");
            gb.Property(guest => guest.Id)
                .HasColumnName("GuestId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => GuestId.Create(value));
        });
        
        builder.Metadata.FindNavigation(nameof(User.Guests))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}