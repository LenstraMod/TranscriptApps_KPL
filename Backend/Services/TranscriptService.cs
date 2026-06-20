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

        //fungsi yg melakukan generate transcript saat audio sesi selesai
        public async Task<Transcript> GenerateTranscript(string scheduleId, byte[] audioBytes)
        {
            bool exist = _transcripts.Any(t => t.scheduleId == scheduleId);
            if (exist) return _transcripts.First(t => t.scheduleId == scheduleId);

            bool updated = _scheduleService.ApplyEvent(scheduleId, "SelesaiRekam");
            if (!updated)
                throw new InvalidOperationException("Schedule belum di-booking atau sudah selesai, tidak bisa generate transcript");

            //buat record transcript dulu dengan status awal, lalu jalankan event "MulaiProses"
            var transcript = new Transcript
            {
                id = GetNextId(),
                scheduleId = scheduleId,
                createdAt = DateTime.Now
            };

            transcript.Apply("MulaiProses"); // BelumDiproses -> SedangDiproses

            _transcripts.Add(transcript);
            _jsonHelper.SaveJson(fileName, _transcripts);

            try
            {
                var (technical, simplified) = await _geminiService.TranscribedAudio(audioBytes);

                transcript.technicalTranscript = technical;
                transcript.simplifiedTranscripts = simplified;
                transcript.Apply("GeminiSukses"); // SedangDiproses -> Selesai
            }
            catch (Exception)
            {
                transcript.Apply("GeminiGagal"); // SedangDiproses -> Gagal
                _jsonHelper.SaveJson(fileName, _transcripts);
                throw; // tetap lempar error supaya controller bisa kasih response gagal
            }

            _jsonHelper.SaveJson(fileName, _transcripts);
            return transcript;
        }

        // Dipanggil kalau status transcript "Gagal" dan ingin diproses ulang
        public async Task<Transcript> RetryTranscript(string scheduleId, byte[] audioBytes)
        {
            var transcript = _transcripts.FirstOrDefault(t => t.scheduleId == scheduleId);
            if (transcript == null)
                throw new InvalidOperationException("Transcript tidak ditemukan");

            transcript.Apply("Retry"); // Gagal -> SedangDiproses

            try
            {
                var (technical, simplified) = await _geminiService.TranscribedAudio(audioBytes);
                transcript.technicalTranscript = technical;
                transcript.simplifiedTranscripts = simplified;
                transcript.Apply("GeminiSukses");
            }
            catch (Exception)
            {
                transcript.Apply("GeminiGagal");
                _jsonHelper.SaveJson(fileName, _transcripts);
                throw;
            }

            _jsonHelper.SaveJson(fileName, _transcripts);
            return transcript;
        }

        //Fungsi untuk mengambil transcript sesuai ID dan rolenya
        public string? GetTranscript(string scheduleId, string role)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");
            Contract.Requires(!string.IsNullOrWhiteSpace(role), "Role tidak boleh kosong");

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