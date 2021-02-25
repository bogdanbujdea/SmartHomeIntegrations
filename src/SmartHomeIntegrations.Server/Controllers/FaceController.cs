using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomeIntegrations.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOfficePersons()
        {
            return Ok(new
            {
                message = "Hello world"
            });
        }
    }
}
