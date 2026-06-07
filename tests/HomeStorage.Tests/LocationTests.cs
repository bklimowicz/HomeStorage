using HomeStorage.Core.Entities;
using HomeStorage.Core.Exceptions;
using HomeStorage.Core.ValueObjects;
using Shouldly;

namespace HomeStorage.Tests;

public class LocationTests
{
    [Fact]
    public void create_location_with_name_should_succeed()
    {
        var location = Location.Create("Drawer in the wardrobe");

        location.LocationName.Value.ShouldBe("Drawer in the wardrobe");
    }

    [Fact]
    public void create_location_with_empty_name_should_throw_exception()
    {
        Should.Throw<LocationNameEmptyException>(() => Location.Create(""));
    }

    [Fact]
    public void update_name_with_name_provided_should_succeed()
    {
        var location = Location.Create("Old name");

        location.UpdateName("New name");

        location.LocationName.Value.ShouldBe("New name");
    }

    [Fact]
    public void update_name_with_empty_name_should_throw_exception()
    {
        var location = Location.Create("Old name");

        Should.Throw<LocationNameEmptyException>(() => location.UpdateName(string.Empty));
    }

    [Fact]
    public void to_dto_should_map_name()
    {
        var location = Location.Create("Garage shelf");

        var dto = location.ToDto();

        dto.LocationName.ShouldBe("Garage shelf");
    }
}
