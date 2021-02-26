namespace SmartHomeIntegrations.Server.Infrastructure
{

    public class ServerSettings
    {
        public string ImagesBaseUrl { get; set; }
        public string FaceSubscriptionKey { get; set; }
        public string FaceEndpoint { get; set; }
        public string HassURL { get; set; }
        public string HassApiKey { get; set; }
    }
}
