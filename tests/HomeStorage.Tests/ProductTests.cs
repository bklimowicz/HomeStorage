using HomeStorage.Core.Entities;
using HomeStorage.Core.Exceptions;
using HomeStorage.Core.ValueObjects;
using Shouldly;

namespace HomeStorage.Tests;

public class ProductTests
{
    private const int LocationId = 1;

    private static Product CreateValidProduct() =>
        Product.Create(Guid.NewGuid(), "Test name", 1.0m, LocationId);

    [Fact]
    public void create_product_with_empty_name_should_throw_exception()
    {
        Should.Throw<InvalidNameException>(() =>
            Product.Create(Guid.NewGuid(), "", 1.0m, LocationId));
    }

    [Fact]
    public void create_product_with_zero_quantity_should_throw_exception()
    {
        Should.Throw<InvalidQuantityException>(() =>
            Product.Create(Guid.NewGuid(), "Test name", 0m, LocationId));
    }

    [Fact]
    public void create_product_with_negative_quantity_should_throw_exception()
    {
        Should.Throw<InvalidQuantityException>(() =>
            Product.Create(Guid.NewGuid(), "Test name", -1.0m, LocationId));
    }

    [Fact]
    public void create_product_with_valid_data_should_succeed()
    {
        var product = Product.Create(Guid.NewGuid(), "Test name", 2.0m, LocationId);

        product.Name.Value.ShouldBe("Test name");
        product.Quantity.Value.ShouldBe(2.0m);
        product.LocationId.ShouldBe(LocationId);
    }

    [Fact]
    public void update_name_with_name_provided_should_succeed()
    {
        var product = CreateValidProduct();

        product.UpdateName("New name");

        product.Name.Value.ShouldBe("New name");
    }

    [Fact]
    public void update_name_with_empty_name_should_throw_exception()
    {
        var product = CreateValidProduct();

        Should.Throw<InvalidNameException>(() => product.UpdateName(string.Empty));
    }

    [Fact]
    public void update_quantity_with_positive_quantity_should_succeed()
    {
        var product = CreateValidProduct();

        product.UpdateQuantity(2.0m);

        product.Quantity.Value.ShouldBe(2.0m);
    }

    [Fact]
    public void update_quantity_with_negative_quantity_should_throw_exception()
    {
        var product = CreateValidProduct();

        Should.Throw<InvalidQuantityException>(() => product.UpdateQuantity(-1.0m));
    }

    [Fact]
    public void move_to_location_should_change_location()
    {
        var product = CreateValidProduct();

        product.MoveToLocation(42);

        product.LocationId.ShouldBe(42);
    }

    [Fact]
    public void to_dto_should_map_all_fields()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Paper", 5m, LocationId, "A4", "Acme");

        var dto = product.ToDto();

        dto.Id.ShouldBe(id);
        dto.Name.ShouldBe("Paper");
        dto.Quantity.ShouldBe(5m);
        dto.LocationId.ShouldBe(LocationId);
        dto.Description.ShouldBe("A4");
        dto.Producer.ShouldBe("Acme");
    }
}
