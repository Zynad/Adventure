using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class TileConfiguration : IEntityTypeConfiguration<Tile>
{
    public void Configure(EntityTypeBuilder<Tile> builder)
    {
        builder.HasIndex(t => new { t.ZoneId, t.X, t.Y })
            .IsUnique();

        builder.HasOne(t => t.Zone)
            .WithMany(z => z.Tiles)
            .HasForeignKey(t => t.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
