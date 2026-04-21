using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Mariage.Infrastructure.Persistence;

public class MariageDbContext(DbContextOptions<MariageDbContext> options) : DbContext(options)
{
    public DbSet<Gift> Gifts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Picture> Pictures { get; set; } = null!;
    public DbSet<Accommodation> Accommodations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(MariageDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}