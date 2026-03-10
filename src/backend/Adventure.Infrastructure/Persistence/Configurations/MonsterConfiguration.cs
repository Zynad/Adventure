using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class MonsterConfiguration : IEntityTypeConfiguration<Monster>
{
    public void Configure(EntityTypeBuilder<Monster> builder)
    {
        builder.HasOne(m => m.LootTable)
            .WithMany()
            .HasForeignKey(m => m.LootTableId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
