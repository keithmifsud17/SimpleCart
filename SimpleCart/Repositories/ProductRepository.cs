using Microsoft.Extensions.Logging;
using SimpleCart.Context;
using SimpleCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCart.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly SimpleCartContext _context;
        private readonly ILogger _logger;

        public ProductRepository(SimpleCartContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("IProductRepository");
        }

        public IQueryable<Product> GetAll()
        {
            _logger.LogInformation("ProductRepository.GetAll");
            return _context.Products;
        }
    }
}
