using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class QuestConfiguration : IEntityTypeConfiguration<Quest>
{
    public void Configure(EntityTypeBuilder<Quest> builder)
    {
        builder.HasOne<Npc>()
            .WithMany()
            .HasForeignKey(q => q.QuestGiverNpcId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Faction>()
            .WithMany()
            .HasForeignKey(q => q.FactionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(q => q.RewardItemId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(q => q.Objectives)
            .WithOne()
            .HasForeignKey(o => o.QuestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
