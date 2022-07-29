using eCommerce.API.Controllers;
using eCommerce.API.Database;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public List<string> Get() => FakeDatabase.CartList;

        [HttpPost("Add")]
        public string Add(string name)
        {
            FakeDatabase.CartList.Add(name);
            return name;
        }

        [HttpPost("Delete")]
        public string Delete(string name)
        {
            FakeDatabase.CartList.Remove(name);
            return name;
        }
    }
}
