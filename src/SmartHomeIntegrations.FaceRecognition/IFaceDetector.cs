using System.Threading.Tasks;

namespace SmartHomeIntegrations.FaceRecognition
{
    public interface IFaceDetector
    {
        Task<FaceRecognitionStatus> RecognizeFaces(string imagePath);
    }
}