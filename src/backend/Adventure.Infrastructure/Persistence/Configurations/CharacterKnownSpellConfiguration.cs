using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class CharacterKnownSpellConfiguration : IEntityTypeConfiguration<CharacterKnownSpell>
{
    public void Configure(EntityTypeBuilder<CharacterKnownSpell> builder)
    {
        builder.HasKey(cks => new { cks.CharacterId, cks.SpellId });

        builder.HasOne(cks => cks.Spell)
            .WithMany()
            .HasForeignKey(cks => cks.SpellId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
