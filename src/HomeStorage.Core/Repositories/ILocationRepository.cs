using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Repositories;

public interface ILocationRepository
{
    public Location Get(LocationName locationName);
    public IEnumerable<Location> GetAll();
    public void Add(Location location);
    public void Update(Location location);
    public void Delete(Location location);
}