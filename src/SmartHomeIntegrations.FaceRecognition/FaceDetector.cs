using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Core.Infrastructure;

namespace SmartHomeIntegrations.FaceRecognition
{
    public class FaceDetector : IFaceDetector
    {
        private readonly IOptions<ServerSettings> _serverSettings;
        private static IFaceClient _client;
        private static Guid _bogdanId;

        public FaceDetector(IOptions<ServerSettings> serverSettings)
        {
            _serverSettings = serverSettings;
            _bogdanId = Guid.Parse(serverSettings.Value.BogdanFaceId);
            _client = Authenticate(_serverSettings.Value.FaceEndpoint, _serverSettings.Value.FaceSubscriptionKey);
        }

        public async Task BuildTrainingModel()
        {
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            var images = new List<string>();
            for (int i = 1; i <= 7; i++)
            {
                images.Add($"{i}.png");
            }
            await CreatePerson(_client, "Bogdan Bujdea", images, _serverSettings.Value.ImagesBaseUrl,
                RECOGNITION_MODEL3);
        }

        public IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        private async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string recognitionModel)
        {
            var imageStream = await new HttpClient().GetStreamAsync(url);
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(imageStream, recognitionModel: recognitionModel, detectionModel: DetectionModel.Detection02);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }

        public async Task CreatePerson(IFaceClient client, string name, List<string> images, string baseUrl, string recognitionModel)
        {
            await client.PersonGroup.CreateAsync(_serverSettings.Value.FamilyGroupId, _serverSettings.Value.FamilyGroupId, recognitionModel: recognitionModel);
            Person person = await client.PersonGroupPerson.CreateAsync(_serverSettings.Value.FamilyGroupId, name);
            _bogdanId = person.PersonId;
            foreach (var similarImage in images)
            {
                await client.PersonGroupPerson.AddFaceFromUrlAsync(_serverSettings.Value.FamilyGroupId, person.PersonId,
                    $"{baseUrl}{similarImage}", similarImage);
            }
            await client.PersonGroup.TrainAsync(_serverSettings.Value.FamilyGroupId);

            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(_serverSettings.Value.FamilyGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }
        }

        public async Task<FaceRecognitionStatus> RecognizeFaces(string imagePath)
        {
            List<Guid?> sourceFaceIds = new List<Guid?>();
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(_client, imagePath, RecognitionModel.Recognition03);

            if (detectedFaces.Count <= 0) return new FaceRecognitionStatus();
            foreach (var detectedFace in detectedFaces)
            {
                sourceFaceIds.Add(detectedFace.FaceId.Value);
            }
            var identifyResults = await _client.Face.IdentifyAsync(sourceFaceIds, _serverSettings.Value.FamilyGroupId);

            foreach (var identifyResult in identifyResults.Where(r => r.Candidates.Any()))
            {
                Person personX = await _client.PersonGroupPerson.GetAsync(_serverSettings.Value.FamilyGroupId, identifyResult.Candidates[0].PersonId);
                Console.WriteLine($"Person '{personX.Name}' is identified for face in: 1.jpg - {identifyResult.FaceId}," +
                                  $" confidence: {identifyResult.Candidates[0].Confidence}.");
            }

            var isMe = identifyResults.Any(r => r.Candidates.Any(c => c.PersonId == _bogdanId));
            return new FaceRecognitionStatus
            {
                IsMe = isMe,
                DetectedPersons = detectedFaces.Count,
                IdentifiedPersons = identifyResults.Count(r => r.Candidates.Any())
            };

        }
    }
}
