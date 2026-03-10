using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class ZoneConnectionConfiguration : IEntityTypeConfiguration<ZoneConnection>
{
    public void Configure(EntityTypeBuilder<ZoneConnection> builder)
    {
        builder.HasOne<Zone>()
            .WithMany(z => z.Connections)
            .HasForeignKey(zc => zc.FromZoneId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Zone>()
            .WithMany()
            .HasForeignKey(zc => zc.ToZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
