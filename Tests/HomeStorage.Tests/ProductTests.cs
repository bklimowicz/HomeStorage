using HomeStorage.Domain.Model;
using HomeStorage.Domain.Exceptions;
using Shouldly;

namespace HomeStorage.Tests;

public class ProductTests
{
    private Location _location = new Location("Test location", null);
    [Fact]
    public void create_product_with_empty_name_should_throw_exception()
    {
        Should.Throw<InvalidNameException>(() => new Product("", _location, 1.0m));
    }
    
    [Fact]
    public void create_product_without_location_should_throw_exception()
    {
        Should.Throw<InvalidLocationException>(() => new Product("Test name", null, 1.0m));
    }
    
    [Fact]
    public void create_product_with_negative_quantity_should_throw_exception()
    {
        Should.Throw<InvalidQuantityException>(() => new Product("Test name", _location, -1.0m));
    }
    
    [Fact]
    public void update_name_name_provided_should_succeed()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        product.UpdateName("New name");
        
        product.Name.ShouldBe("New name");
    }

    [Fact]
    public void update_quantity_with_positive_quantity_should_succeed()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        product.UpdateQuantity(2.0m);
        
        product.Quantity.ShouldBe(2.0m);
    }
    
    [Fact]
    public void update_location_with_new_location_should_succeed()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        product.UpdateLocation(new Location("New location", null));
        
        product.Location.Name.ShouldBe("New location");
    }

    [Fact]
    public void update_name_empty_name_should_throw_exception()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        Should.Throw<InvalidNameException>(() => product.UpdateName(string.Empty));
    }

    [Fact]
    public void update_quantity_with_negative_quantity_should_throw_exception()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        Should.Throw<InvalidQuantityException>(() => product.UpdateQuantity(-1.0m));
    }
    
    [Fact]
    public void update_location_with_empty_location_should_throw_exception()
    {
        var product = new Product("Test name", _location, 1.0m);
        
        Should.Throw<InvalidQuantityException>(() => product.UpdateQuantity(-1.0m));
    }
}