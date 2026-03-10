using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class EquippedItemConfiguration : IEntityTypeConfiguration<EquippedItem>
{
    public void Configure(EntityTypeBuilder<EquippedItem> builder)
    {
        builder.HasIndex(e => new { e.CharacterId, e.SlotType })
            .IsUnique();
    }
}
