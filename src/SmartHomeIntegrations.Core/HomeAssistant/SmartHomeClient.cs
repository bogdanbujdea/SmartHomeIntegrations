using System.Threading.Tasks;
using HADotNet.Core;
using HADotNet.Core.Clients;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Core.Infrastructure;

namespace SmartHomeIntegrations.Core.HomeAssistant
{
    public interface ISmartHomeClient
    {
        Task SetVar(string name, string newValue);
    }

    public class SmartHomeClient : ISmartHomeClient
    {
        private readonly IOptions<ServerSettings> _serverSettings;

        public SmartHomeClient(IOptions<ServerSettings> serverSettings)
        {
            _serverSettings = serverSettings;
        }

        public async Task SetVar(string name, string newValue)
        {
            if (ClientFactory.IsInitialized == false)
            {
                ClientFactory.Initialize(_serverSettings.Value.HassURL, _serverSettings.Value.HassApiKey);
            }

            var serviceClient = ClientFactory.GetClient<ServiceClient>();

            await serviceClient.CallService("var", "set", new
            {
                entity_id = name,
                value = newValue
            });
        }
    }
}
