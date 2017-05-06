using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SimpleCart.Models;
using SimpleCart.Repositories;
using System;
using System.Linq;

namespace SimpleCart.Controllers
{
    [Produces("application/json")]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartRepository _repository;
        private readonly IMemoryCache _cache;

        public ShoppingCartsController(IShoppingCartRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        //GET: api/ShoppingCarts
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_cache.GetOrCreate("ShoppingCart", e => { e.SlidingExpiration = TimeSpan.FromMinutes(15); return _repository.GetAll().Select(p => new { ID = p.ID, Code = p.Product.Code, Description = p.Product.Description, Price = p.Product.Price, UserId = p.UserId, Quantity = p.Quantity, Total = p.Quantity * p.Product.Price }).ToList(); }));
        }

        // POST: api/ShoppingCarts
        [HttpPost]
        public IActionResult Post([FromBody]ShoppingCart value)
        {
            if (_repository.Insert(value))
            {
                _cache.Remove("ShoppingCart"); //A new item has been added, thus invalidating the current cache.
                return Created("/api/ShoppingCarts", value);
            }
            return NotFound();
        }

        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _repository.Get(id);
            if (item != null)
            {
                if(_repository.Delete(item))
                {
                    _cache.Remove("ShoppingCart"); //An item has been deleted, thus invalidating the current cache.
                    return Ok();
                }
            }
            return NotFound();
        }

        // DELETE: api/ShoppingCarts
        [HttpDelete()]
        public IActionResult Delete()
        {
            if (_repository.DeleteAll())
            {
                _cache.Remove("ShoppingCart"); //All items have been deleted, thus invalidating the current cache.
                return Ok();
            }
            return NotFound();
        }
    }
}
