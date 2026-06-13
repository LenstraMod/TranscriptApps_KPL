using Backend.Models;
using JsonHelperLibrary;

namespace Backend.Services
{
    public class TranscriptService
    {
        private readonly JsonHelper _jsonHelper;
        private readonly GeminiService _geminiService;
        private List<Transcript> _transcripts;

        private const string fileName = "transcripts.json";

        public TranscriptService(JsonHelper jsonHelper, GeminiService geminiService) 
        {
            _jsonHelper = jsonHelper;
            _geminiService = geminiService;
            _transcripts = _jsonHelper.LoadJson<Transcript>(fileName);
        }

        public async Task<Transcript> GenerateTranscript(int appointmentIds, byte[] audioBytes) 
        {
            bool exist = _transcripts.Any(t => t.appointmentId == appointmentIds);

            if (exist) return _transcripts.First(t => t.appointmentId == appointmentIds);

            var (technical, simplified) = await _geminiService.TranscribedAudio(audioBytes);

            var transcript = new Transcript
            {
                id = GetNextId(),
                appointmentId = appointmentIds,
                technicalTranscript = technical,
                simplifiedTranscripts = simplified,
                createdAt = DateTime.Now
            };

            _transcripts.Add(transcript);
            _jsonHelper.SaveJson(fileName, _transcripts);

            return transcript;
         
        }

        public string? GetTranscript(int appointmentId, string role) 
        {
            var transcript = _transcripts.FirstOrDefault(t => t.appointmentId == appointmentId);

            if(transcript == null) return null;

            return role == "Psikolog" ? transcript.technicalTranscript : transcript.simplifiedTranscripts;
        }

        public List<Transcript> GetAll() { return _transcripts; }

        public int GetNextId() {
            return _transcripts.Any() ? _transcripts.Max(t => t.id) + 1 : 0;
        }

    }
}
