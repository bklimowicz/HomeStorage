using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.DAL.Repositories;

public class PostgresDbLocationRepository : ILocationRepository
{
    public Location Get(LocationName locationName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(Location location)
    {
        throw new NotImplementedException();
    }

    public void Update(Location location)
    {
        throw new NotImplementedException();
    }

    public void Delete(Location location)
    {
        throw new NotImplementedException();
    }
}