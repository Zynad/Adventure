using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class SaveGameConfiguration : IEntityTypeConfiguration<SaveGame>
{
    public void Configure(EntityTypeBuilder<SaveGame> builder)
    {
        builder.Property(s => s.GameStateJson).HasColumnType("TEXT");
    }
}
