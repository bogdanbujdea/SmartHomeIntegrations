using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Core.HomeAssistant;
using SmartHomeIntegrations.Core.Infrastructure;
using SmartHomeIntegrations.FaceRecognition;

namespace SmartHomeIntegrations.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private readonly IOptions<ServerSettings> _serverSettings;
        private readonly IFaceDetector _faceDetector;
        private readonly ISmartHomeClient _smartHomeClient;

        public FaceController(IOptions<ServerSettings> serverSettings, IFaceDetector faceDetector, ISmartHomeClient smartHomeClient)
        {
            _serverSettings = serverSettings;
            _faceDetector = faceDetector;
            _smartHomeClient = smartHomeClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetOfficePersons()
        {
            try
            {

                var faceRecognitionStatus = await _faceDetector.RecognizeFaces(_serverSettings.Value.OfficeCameraSnapshot);
                if (faceRecognitionStatus.IsMe)
                {
                    await _smartHomeClient.SetVar("var.office_person", "Bogdan");

                }
                await _smartHomeClient.SetVar("var.office_people_count", faceRecognitionStatus.DetectedPersons.ToString());
                return Ok(faceRecognitionStatus);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest((e as APIErrorException)?.Body?.Error?.Message);
            }
        }
    }
}
