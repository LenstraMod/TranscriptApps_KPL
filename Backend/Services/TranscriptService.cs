using Backend.Models;
using JsonHelperLibrary;

namespace Backend.Services
{
    public class TranscriptService
    {
        private readonly JsonHelper _jsonHelper;
        private readonly GeminiService _geminiService;
        private readonly ScheduleService _scheduleService;
        private List<Transcript> _transcripts;

        private const string fileName = "transcripts.json";

        public TranscriptService(JsonHelper jsonHelper, GeminiService geminiService, ScheduleService scheduleService)
        {
            _jsonHelper = jsonHelper;
            _geminiService = geminiService;
            _scheduleService = scheduleService;
            _transcripts = _jsonHelper.LoadJson<Transcript>(fileName);
        }

        public async Task<Transcript> GenerateTranscript(string scheduleId, byte[] audioBytes)
        {
            bool exist = _transcripts.Any(t => t.scheduleId == scheduleId);
            if (exist) return _transcripts.First(t => t.scheduleId == scheduleId);

            bool updated = _scheduleService.ApplyEvent(scheduleId, "SelesaiRekam");
            if (!updated)
                throw new InvalidOperationException("Schedule belum di-booking atau sudah selesai, tidak bisa generate transcript");

            var (technical, simplified) = await _geminiService.TranscribedAudio(audioBytes);

            var transcript = new Transcript
            {
                id = GetNextId(),
                scheduleId = scheduleId,
                technicalTranscript = technical,
                simplifiedTranscripts = simplified,
                createdAt = DateTime.Now
            };

            _transcripts.Add(transcript);
            _jsonHelper.SaveJson(fileName, _transcripts);

            return transcript;
        }

        public string? GetTranscript(string scheduleId, string role)
        {
            var transcript = _transcripts.FirstOrDefault(t => t.scheduleId == scheduleId);
            if (transcript == null) return null;
            return role == "Psikolog" ? transcript.technicalTranscript : transcript.simplifiedTranscripts;
        }

        public List<Transcript> GetAll() => _transcripts;

        public int GetNextId() => _transcripts.Any() ? _transcripts.Max(t => t.id) + 1 : 0;
    }
}