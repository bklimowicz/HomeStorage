using HomeStorage.Core.DTOs;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Entities;

public class Location
{
    private Location(int id, LocationName locationName)
    {
        Id = id;
        LocationName = locationName;
    }

    public int Id { get; private set; }
    public LocationName LocationName { get; private set; }

    public static Location Create(LocationName locationName) => new(default, locationName);

    public void UpdateName(LocationName locationName) => LocationName = locationName;

    public LocationDto ToDto() => new()
    {
        Id = Id,
        LocationName = LocationName.Value
    };
}
