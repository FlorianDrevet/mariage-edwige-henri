using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mariage.Infrastructure.Persistence.Configurations;

public class AccommodationConfiguration : IEntityTypeConfiguration<Accommodation>
{
    public void Configure(EntityTypeBuilder<Accommodation> builder)
    {
        ConfigureAccommodationsTable(builder);
        ConfigureBookings(builder);
    }

    private void ConfigureBookings(EntityTypeBuilder<Accommodation> builder)
    {
        builder.OwnsMany(a => a.Bookings, bb =>
        {
            bb.ToTable("AccommodationBookings");
            bb.WithOwner().HasForeignKey("AccommodationId");
            bb.HasKey("Id", "AccommodationId");
            bb.Property(booking => booking.Id)
                .HasColumnName("AccommodationBookingId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AccommodationBookingId.Create(value));
            bb.Property(booking => booking.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));
            bb.Property(booking => booking.BookingStatus)
                .HasConversion<string>();
            bb.Property(booking => booking.TotalAmount)
                .HasPrecision(18, 2);
        });

        builder.Metadata.FindNavigation(nameof(Accommodation.Bookings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureAccommodationsTable(EntityTypeBuilder<Accommodation> builder)
    {
        builder.ToTable("Accommodations");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => AccommodationId.Create(value));
        builder.Property(a => a.PricePerPersonPerNight)
            .HasPrecision(18, 2);
    }
}
