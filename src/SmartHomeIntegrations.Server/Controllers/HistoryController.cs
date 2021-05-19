using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeIntegrations.Core.Entities;
using SmartHomeIntegrations.Core.HomeAssistant;

namespace SmartHomeIntegrations.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController: ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISmartHomeClient _smartHomeClient;

        public HistoryController(ApplicationDbContext dbContext, ISmartHomeClient smartHomeClient)
        {
            _dbContext = dbContext;
            _smartHomeClient = smartHomeClient;
        }

        [HttpPost("update-standing-up-sensors")]
        public async Task<IActionResult> GetStandingUpInfo()
        {
            try
            {
                var oneWeekAgo = DateTime.UtcNow.Subtract(TimeSpan.FromDays(6));

                var standingUp = await _dbContext.States
                    .Where(s => s.EntityId == "sensor.standing_up_time_today" && s.LastChanged >= oneWeekAgo)
                    .OrderByDescending(s => s.LastChanged)
                    .ToListAsync();
                var days = standingUp
                    .GroupBy(s => s.LastChanged.AddHours(3).Day)
                    .Select(g => g.OrderByDescending(s => s.LastChanged).FirstOrDefault())
                    .ToList();

                foreach (var day in days)
                {
                    var i = DateTime.UtcNow.Subtract(day.LastUpdated).Days;
                    if (i == 0)
                        continue;
                    await _smartHomeClient.SetVar($"var.standing_up_{i}_days_ago", days[i].CurrentState);
                }
                return Ok(days);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
