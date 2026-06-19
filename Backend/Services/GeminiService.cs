using JsonHelperLibrary;
using System.Globalization;
using System.Text;

namespace Backend.Services
{
    //Fungsi untuk menanganan service gemini API.
    public class GeminiService
    {
        //Kerangka yang dibutuhkan untuk mengirim request ke gemini API
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string Model = "gemini-2.5-flash";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

        //Prompt Engineering untuk transcript yang diinginkan
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

        //Fungsi koneksi ke gemini dan lakukan generasi transkrip
        public async Task<(string Technical, string Simplified)> TranscribedAudio(byte[] audioBytes)
        {
            var technical = await CallGemini(audioBytes, TechnicalPrompt);
            var simplified = await CallGemini(audioBytes, SimplifiedPrompt);

            return (technical, simplified);
        }

        //Fungsi detail bagaimana pengiriman request ke gemini dilakukan
        public async Task<string> CallGemini(byte[] audioBytes, string prompt)
        {

            //Konversi base64audio ke base64string
            string base64audio = Convert.ToBase64String(audioBytes);
            //Siapkanjson final yang akan dikirim ke gemini
            string json = GeminiResponse.BuildAudioRequest(base64audio, prompt);

            //Base URL serta headernya
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"{BaseUrl}/{Model}:generateContent?key={_apiKey}";

            //Kirim request ke gemini dan tunggu sampai selesai. karena async maka proses lain bisa dilakukan
            var response = await _httpClient.PostAsync(url, content);
            string responseJson = await response.Content.ReadAsStringAsync();


            //Jika gagal maka throw exception.
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API error {(int)response.StatusCode}: {responseJson}");
            }

            //Return transcript yang ada
            return GeminiResponse.ParseTranscript(responseJson);
        }
    }
}
