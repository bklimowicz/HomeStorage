using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeStorage.Core.DAL.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ProductId(x));
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new ProductName(x));
        builder.Property(x => x.Quantity)
            .HasConversion(x => x.Value, x => new Quantity(x));
        builder.Property(x => x.Description)
            .HasConversion(x => x.Value, x => new Description(x));
        builder.Property(x => x.Producer)
            .HasConversion(x => x.Value, x => new Producer(x));
    }
}