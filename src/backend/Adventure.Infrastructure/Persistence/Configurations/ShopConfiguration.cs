using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasMany(s => s.Inventory)
            .WithOne(e => e.Shop)
            .HasForeignKey(e => e.ShopId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
