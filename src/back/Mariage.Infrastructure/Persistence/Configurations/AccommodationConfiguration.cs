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
        ConfigureAssignments(builder);
    }

    private void ConfigureAssignments(EntityTypeBuilder<Accommodation> builder)
    {
        builder.OwnsMany(a => a.Assignments, ab =>
        {
            ab.ToTable("AccommodationAssignments");
            ab.WithOwner().HasForeignKey("AccommodationId");
            ab.HasKey("Id", "AccommodationId");
            ab.Property(assignment => assignment.Id)
                .HasColumnName("AccommodationAssignmentId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AccommodationAssignmentId.Create(value));
            ab.Property(assignment => assignment.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));
            ab.Property(assignment => assignment.ResponseStatus)
                .HasConversion<string>();
        });

        builder.Metadata.FindNavigation(nameof(Accommodation.Assignments))!
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
        builder.Property(a => a.Price)
            .HasPrecision(18, 2);
    }
}
