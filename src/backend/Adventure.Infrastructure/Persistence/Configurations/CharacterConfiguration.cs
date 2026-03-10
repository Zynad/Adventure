using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

        builder.Ignore(c => c.DomainEvents);

        builder.HasMany(c => c.Inventory)
            .WithOne(i => i.Character)
            .HasForeignKey(i => i.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Equipment)
            .WithOne(e => e.Character)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.KnownSpells)
            .WithOne(ks => ks.Character)
            .HasForeignKey(ks => ks.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.SpellSlotsJson).HasMaxLength(500);

        builder.HasMany(c => c.Quests)
            .WithOne()
            .HasForeignKey(q => q.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Reputations)
            .WithOne()
            .HasForeignKey(r => r.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Professions)
            .WithOne()
            .HasForeignKey(p => p.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
