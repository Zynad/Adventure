using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<Item>("Item")
            .HasValue<Equipment>("Equipment")
            .HasValue<Consumable>("Consumable")
            .HasValue<CraftingMaterial>("CraftingMaterial");
    }
}
