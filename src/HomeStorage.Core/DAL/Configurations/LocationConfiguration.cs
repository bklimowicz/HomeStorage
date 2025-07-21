using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeStorage.Core.DAL.Configurations;

internal sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.LocationName);
        builder.Property(x => x.LocationName)
            .HasConversion(x => x.Value, x => new LocationName(x));
    }
}