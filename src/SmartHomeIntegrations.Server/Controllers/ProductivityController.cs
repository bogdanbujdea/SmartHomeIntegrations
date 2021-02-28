using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHomeIntegrations.Core.Integrations;

namespace SmartHomeIntegrations.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductivityController: ControllerBase
    {
        private readonly IProductivityService _productivityService;

        public ProductivityController(IProductivityService productivityService)
        {
            _productivityService = productivityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductivityScore()
        {
            var productivityScore = await _productivityService.GetProductivityScore();
            return Ok(new { productivityScore });
        }
    }
}
