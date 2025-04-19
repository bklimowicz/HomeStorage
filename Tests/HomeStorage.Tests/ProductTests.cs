// using HomeStorage.Domain.Entities;
// using HomeStorage.Domain.Exceptions;
// using HomeStorage.Domain.ValueObjects;
// using Shouldly;
//
// namespace HomeStorage.Tests;
//
// public class ProductTests
// {
//     private readonly Location _location = new Location("Test location");
//     [Fact]
//     public void create_product_with_empty_name_should_throw_exception()
//     {
//         Should.Throw<InvalidNameException>(() => new Product(Guid.NewGuid(), "", 1.0m));
//     }
//     
//     [Fact]
//     public void create_product_with_negative_quantity_should_throw_exception()
//     {
//         Should.Throw<InvalidQuantityException>(() => new Product(Guid.NewGuid(),"Test name", -1.0m));
//     }
//     
//     [Fact]
//     public void update_name_name_provided_should_succeed()
//     {
//         var product = new Product(Guid.NewGuid(),"Test name", 1.0m);
//         
//         product.UpdateName("New name");
//         
//         product.Name.ShouldBe<ProductName>("New name");
//     }
//
//     [Fact]
//     public void update_quantity_with_positive_quantity_should_succeed()
//     {
//         var product = new Product(Guid.NewGuid(),"Test name", 1.0m);
//         
//         product.UpdateQuantity(2.0m);
//         
//         product.Quantity.ShouldBe<Quantity>(2.0m);
//     }
//
//     [Fact]
//     public void update_name_empty_name_should_throw_exception()
//     {
//         var product = new Product(Guid.NewGuid(),"Test name", 1.0m);
//         
//         Should.Throw<InvalidNameException>(() => product.UpdateName(string.Empty));
//     }
//
//     [Fact]
//     public void update_quantity_with_negative_quantity_should_throw_exception()
//     {
//         var product = new Product(Guid.NewGuid(),"Test name", 1.0m);
//         
//         Should.Throw<InvalidQuantityException>(() => product.UpdateQuantity(-1.0m));
//     }
//     
//     [Fact]
//     public void update_location_with_empty_location_should_throw_exception()
//     {
//         var product = new Product(Guid.NewGuid(),"Test name", 1.0m);
//         
//         Should.Throw<InvalidQuantityException>(() => product.UpdateQuantity(-1.0m));
//     }
// }