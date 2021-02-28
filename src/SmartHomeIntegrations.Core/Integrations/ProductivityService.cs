using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Core.Extensions;
using SmartHomeIntegrations.Core.Infrastructure;

namespace SmartHomeIntegrations.Core.Integrations
{
    public interface IProductivityService
    {
        Task<int> GetProductivityScore();
    }

    public class ProductivityService : IProductivityService
    {
        private readonly IOptions<ServerSettings> _serverSettings;
        private readonly HttpClient _httpClient;

        public ProductivityService(IOptions<ServerSettings> serverSettings, HttpClient httpClient)
        {
            _serverSettings = serverSettings;
            _httpClient = httpClient;
        }

        public async Task<int> GetProductivityScore()
        {
            var url = $"https://www.rescuetime.com/widget_window/productivity?schedule=6696531";
            _httpClient.DefaultRequestHeaders.Add("Cookie", _serverSettings.Value.RescueTimeCookie);
            var response = await _httpClient.GetStringAsync(url);
            return response.GetProductivityScore();
        } 
    }

}
