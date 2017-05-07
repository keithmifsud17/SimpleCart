using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SimpleCart.Controllers;
using SimpleCart.Models;
using SimpleCart.Repositories;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SimpleCartUnitTest
{
    public class ProductControllerUnitTest
    {
        private readonly IMemoryCache mockCache;
        private readonly IQueryable<Product> Sample;

        public ProductControllerUnitTest()
        {
            mockCache = new MemoryCache(new MemoryCacheOptions());
            Sample = BuildTestProducts();
        }

        [Fact]
        public void TestGetProducts()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(Sample);

            var controller = new ProductsController(mockRepo.Object, mockCache);
            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultCollection = (List<Product>)okResult.Value;

            Assert.Equal(Sample.Count(), resultCollection.Count);

            for (int i = 0; i < resultCollection.Count; i++)
            {
                Assert.Equal(Sample.ElementAt(i).ID, resultCollection[i].ID);
            }
        }

        private IQueryable<Product> BuildTestProducts()
        {
            var items = new List<Product>();

            items.Add(new Product() { ID = 1, Code = "P1", Description = "D1", Price = 1 });
            items.Add(new Product() { ID = 2, Code = "P2", Description = "D2", Price = 1 });
            items.Add(new Product() { ID = 3, Code = "P3", Description = "D3", Price = 1 });
            items.Add(new Product() { ID = 4, Code = "P4", Description = "D4", Price = 1 });

            return items.AsQueryable();
        }
    }
}
