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

        //Fungsi yg melakukan generate transcript saat audio sesi selesai
        public async Task<Transcript> GenerateTranscript(string scheduleId, byte[] audioBytes)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");
            Contract.Requires(audioBytes != null, "Data audio tidak boleh null");
            Contract.Requires(audioBytes!.Length > 0, "Data audio tidak boleh kosong (0 bytes)");

            // Hapus transcript lama jika ada — rekaman baru selalu menghasilkan transcript baru
            var existing = _transcripts.FirstOrDefault(t => t.scheduleId == scheduleId);
            if (existing != null) _transcripts.Remove(existing);

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

            //Transcript dimasukkan ke dalam List dan simpan ke json
            _transcripts.Add(transcript);
            _jsonHelper.SaveJson(fileName, _transcripts);

            // Postcondition: transcript harus tersimpan dan bisa ditemukan
            Contract.Ensures(_transcripts.Any(t => t.scheduleId == scheduleId),
                "Transcript baru harus ada di list setelah GenerateTranscript");

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