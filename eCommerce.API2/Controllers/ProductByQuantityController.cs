using Library.eCommerce.Models;
using eCommerce.API.EC;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByQuantityController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        public ProductByQuantityController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return new ProductByQuantityEC().Get();
        }
    }
}
