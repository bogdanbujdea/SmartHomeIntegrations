using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Server.Infrastructure;

namespace SmartHomeIntegrations.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private readonly IOptions<ServerSettings> _serverSettings;

        public FaceController(IOptions<ServerSettings> serverSettings)
        {
            _serverSettings = serverSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetOfficePersons()
        {
            return Ok(new 
            {
                message = "Hello world!",
                hassUrl = _serverSettings.Value.HassURL,
                faceEndpoint = _serverSettings.Value.FaceEndpoint
            });
        }
    }
}
