using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class NpcConfiguration : IEntityTypeConfiguration<Npc>
{
    public void Configure(EntityTypeBuilder<Npc> builder)
    {
        builder.HasOne(n => n.Zone)
            .WithMany()
            .HasForeignKey(n => n.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Faction)
            .WithMany()
            .HasForeignKey(n => n.FactionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.Shop)
            .WithOne(s => s.Npc)
            .HasForeignKey<Shop>(s => s.NpcId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
