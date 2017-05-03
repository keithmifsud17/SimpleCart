using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SimpleCart.Repositories;
using System;
using System.Linq;

namespace SimpleCart.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IMemoryCache _cache;

        public ProductsController(IProductRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        //GET: api/Product
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_cache.GetOrCreate("Products", e => { e.SlidingExpiration = TimeSpan.FromMinutes(15); return _repository.GetAll().ToList(); }));
        }
    }
}
