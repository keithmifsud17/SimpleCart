using Microsoft.Extensions.Logging;
using SimpleCart.Context;
using SimpleCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCart.Repositories
{
    public interface IShoppingCartRepository
    {
        IQueryable<ShoppingCart> GetAll();
        ShoppingCart Get(int ID);
        bool Insert(ShoppingCart cart);
        bool Delete(ShoppingCart cart);
        bool DeleteAll();
    }

    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly SimpleCartContext _context;
        private readonly ILogger _logger;

        public ShoppingCartRepository(SimpleCartContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("IShoppingCartRepository");
        }

        public IQueryable<ShoppingCart> GetAll()
        {
            _logger.LogInformation("ShoppingCartRepository.GetAll");
            return _context.ShoppingCarts;
        }

        public ShoppingCart Get(int ID)
        {
            _logger.LogInformation("ShoppingCartRepository.Get");
            return _context.ShoppingCarts.SingleOrDefault(c => c.ID == ID);
        }

        public bool Insert(ShoppingCart cart)
        {
            _logger.LogInformation("ShoppingCartRepository.Insert");
            _context.ShoppingCarts.Add(cart);
            return _context.SaveChanges();
        }

        public bool Delete(ShoppingCart cart)
        {
            _logger.LogInformation("ShoppingCartRepository.Delete");
            _context.ShoppingCarts.Remove(cart);
            return _context.SaveChanges();
        }

        public bool DeleteAll()
        {
            _logger.LogInformation("ShoppingCartRepository.DeleteAll");
            //TODO: Change this to something more efficient.
            _context.ShoppingCarts.RemoveRange(GetAll());
            return _context.SaveChanges();
        }
    }
}
