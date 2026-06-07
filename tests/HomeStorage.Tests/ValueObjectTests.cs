using HomeStorage.Core.Exceptions;
using HomeStorage.Core.ValueObjects;
using Shouldly;

namespace HomeStorage.Tests;

public class ValueObjectTests
{
    [Fact]
    public void product_name_with_empty_value_should_throw_exception()
    {
        Should.Throw<InvalidNameException>(() => new ProductName(""));
    }

    [Fact]
    public void quantity_with_negative_value_should_throw_exception()
    {
        Should.Throw<InvalidQuantityException>(() => new Quantity(-1.0m));
    }

    [Fact]
    public void quantity_with_zero_value_should_be_allowed()
    {
        var quantity = new Quantity(0m);

        quantity.Value.ShouldBe(0m);
    }

    [Fact]
    public void location_name_with_empty_value_should_throw_exception()
    {
        Should.Throw<LocationNameEmptyException>(() => new LocationName(" "));
    }
}
