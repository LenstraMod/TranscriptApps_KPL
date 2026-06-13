using JsonHelperLibrary;
using System.Globalization;
using System.Text;

namespace Backend.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string Model = "gemini-2.5-flash";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

        private const string TechnicalPrompt =
           "You are a clinical psychologist assistant. Transcribe this therapy session " +
           "audio with clinical accuracy. Use proper psychological terminology, note " +
           "emotional states, behavioral patterns, and clinically relevant observations. " +
           "Format it as a professional session record in Indonesian language.";

        private const string SimplifiedPrompt =
            "Transcribe this therapy session audio in simple, friendly Indonesian language " +
            "that a patient can easily understand. Avoid technical jargon. Summarize the " +
            "key points discussed and any advice given in a warm, easy-to-read format.";


        public GeminiService(IConfiguration config)
        {
            _apiKey = config["GeminiApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<(string Technical, string Simplified)> TranscribedAudio(byte[] audioBytes)
        {
            var technical = await CallGemini(audioBytes, TechnicalPrompt);
            var simplified = await CallGemini(audioBytes, SimplifiedPrompt);

            return (technical, simplified);
        }

        public async Task<string> CallGemini(byte[] audioBytes, string prompt)
        {
            string base64audio = Convert.ToBase64String(audioBytes);
            string json = GeminiResponse.BuildAudioRequest(base64audio, prompt);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"{BaseUrl}/{Model}:generationContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(url, content);
            string responseJson = await response.Content.ReadAsStringAsync();

            return GeminiResponse.ParseTranscript(responseJson);
        }
    }
}
