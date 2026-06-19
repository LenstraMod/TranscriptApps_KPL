using Backend.Models;
using JsonHelperLibrary;

namespace Backend.Services
{
    //Service yang menanganani kebutuhan transcript. Mulai dari kasih transcrpt ke AI sampai ambil transcriptnya
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

        //Fungsi yg melakukan generate transcrript saat audio sesi selesai. Ini merupakan implementasi automata
        public async Task<Transcript> GenerateTranscript(string scheduleId, byte[] audioBytes)
        {
            bool exist = _transcripts.Any(t => t.scheduleId == scheduleId);

            //Jika transcript sudah ada, maka kembalikan transcriptnya  
            if (exist) return _transcripts.First(t => t.scheduleId == scheduleId);

            //Implementasi autoamata
            bool updated = _scheduleService.ApplyEvent(scheduleId, "SelesaiRekam");
            if (!updated)
                throw new InvalidOperationException("Schedule belum di-booking atau sudah selesai, tidak bisa generate transcript");

            //Ambil hasil transcript yang ada dari gemini.
            //Fungsi ini async jadi bisa jalan berbarengan dengan fung lainnya
            var (technical, simplified) = await _geminiService.TranscribedAudio(audioBytes);

            //Masukkan hasil gemini transcript ke dalam objek
            var transcript = new Transcript
            {
                id = GetNextId(),
                scheduleId = scheduleId,
                technicalTranscript = technical,
                simplifiedTranscripts = simplified,
                createdAt = DateTime.Now
            };

            //Transcruot dimasukkan ke dalam List dan simpan ke json
            _transcripts.Add(transcript);
            _jsonHelper.SaveJson(fileName, _transcripts);

            return transcript;
        }

        //Funsi untuk mengambil transcript sesuai ID dan rolenya
        public string? GetTranscript(string scheduleId, string role)
        {
            var transcript = _transcripts.FirstOrDefault(t => t.scheduleId == scheduleId);
            if (transcript == null) return null;
            return role == "Psikolog" ? transcript.technicalTranscript : transcript.simplifiedTranscripts;
        }

        //Ambil semua transcript
        public List<Transcript> GetAll() => _transcripts;

        //Ambil id dari idx max pada list trascript
        public int GetNextId() => _transcripts.Any() ? _transcripts.Max(t => t.id) + 1 : 0;
    }
}