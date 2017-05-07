using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SimpleCart.Controllers;
using SimpleCart.Models;
using SimpleCart.Repositories;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System;

namespace SimpleCartUnitTest
{
    public class ShoppingCartControllerUnitTest
    {
        private readonly IMemoryCache mockCache;
        private readonly IQueryable<ShoppingCart> Sample;

        public ShoppingCartControllerUnitTest()
        {
            mockCache = new MemoryCache(new MemoryCacheOptions());
            Sample = BuildTestCart();
        }

        [Fact]
        public void TestGetCarts()
        {
            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(Sample);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultCollection = (List<ShoppingCartView>)okResult.Value;

            Assert.Equal(Sample.Count(), resultCollection.Count);

            for (int i = 0; i < resultCollection.Count; i++)
            {
                Assert.Equal(Sample.ElementAt(i).ID, resultCollection[i].ID);
            }
        }

        [Fact]
        public void TestPost_Valid()
        {
            var cart = new ShoppingCart() { ProductId = 1, Quantity=1, UserId = 1 };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.Insert(cart)).Returns(true);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Post(cart);

            var okResult = Assert.IsType<CreatedResult>(result);
            var resultObject = (ShoppingCart)okResult.Value;

            Assert.Equal(resultObject.ProductId, cart.ProductId);
            Assert.Equal(resultObject.Quantity, cart.Quantity);
            Assert.Equal(resultObject.UserId, cart.UserId);
        }

        [Fact]
        public void TestPost_InvalidProduct()
        {
            var cart = new ShoppingCart() { ProductId = -1, Quantity = 1, UserId = 1 };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.Insert(cart)).Returns(false);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Post(cart);

            var okResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TestPost_InvalidQuantity()
        {
            var cart = new ShoppingCart() { ProductId = 1, Quantity = -1, UserId = 1 };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.Insert(cart)).Returns(true); //This should not be called, however the database does not have any quantity validations so, it would, actually save it.

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            controller.ModelState.AddModelError("Quantity", "Quantity must be larger than 0"); //ModelState error are added when building the request. Calling the method directly does not allow the modelstate to build up its errors.
            var result = controller.Post(cart);

            var okResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TestDelete_Valid()
        {
            var cart = new ShoppingCart() { ID = 1, ProductId = 1, Quantity = 1, UserId = 1 };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.Delete(cart)).Returns(true);
            mockRepo.Setup(r => r.Get(cart.ID)).Returns(cart);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Delete(cart.ID);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void TestDelete_Invalid()
        {
            var cart = new ShoppingCart() { ID = -1, ProductId = 1, Quantity = 1, UserId = 1 };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.Delete(cart)).Returns(false);
            mockRepo.Setup(r => r.Get(cart.ID)).Returns<ShoppingCart>(null);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Delete(cart.ID);

            var okResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TestDeleteAll_Valid()
        {
            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(r => r.DeleteAll()).Returns(true);

            var controller = new ShoppingCartsController(mockRepo.Object, mockCache);
            var result = controller.Delete();

            var okResult = Assert.IsType<OkResult>(result);
        }

        private IQueryable<ShoppingCart> BuildTestCart()
        {
            var items = new List<ShoppingCart>();

            items.Add(new ShoppingCart() { ID = 1, ProductId = 1, Product = new Product() { ID = 1, Code = "P1", Description = "D1", Price = 1 }, UserId = 1, Quantity = 1 });
            items.Add(new ShoppingCart() { ID = 2, ProductId = 2, Product = new Product() { ID = 2, Code = "P2", Description = "D2", Price = 1 }, UserId = 1, Quantity = 1 });
            items.Add(new ShoppingCart() { ID = 3, ProductId = 3, Product = new Product() { ID = 3, Code = "P3", Description = "D3", Price = 1 }, UserId = 1, Quantity = 1 });
            items.Add(new ShoppingCart() { ID = 4, ProductId = 4, Product = new Product() { ID = 4, Code = "P4", Description = "D4", Price = 1 }, UserId = 1, Quantity = 1 });

            return items.AsQueryable();
        }
    }
}
