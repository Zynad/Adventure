using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class SpellConfiguration : IEntityTypeConfiguration<Spell>
{
    public void Configure(EntityTypeBuilder<Spell> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.CastingTime).HasMaxLength(50);
        builder.Property(s => s.Range).HasMaxLength(50);
        builder.Property(s => s.Duration).HasMaxLength(50);
        builder.Property(s => s.DamageDice).HasMaxLength(20);
        builder.Property(s => s.HealingDice).HasMaxLength(20);
    }
}
