using System.Text.Json.Serialization;

namespace Backend.Models
{
    public enum TranscriptStatus { BelumDiproses, SedangDiproses, Selesai, Gagal }

    public class Transcript
    {
        public int id { get; set; }
        public string scheduleId { get; set; }
        public string technicalTranscript { get; set; }
        public string simplifiedTranscripts { get; set; }
        public DateTime createdAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TranscriptStatus Status { get; set; } = TranscriptStatus.BelumDiproses; // inisialisasi awal state

        // tabel transisi (data)
        private static readonly Dictionary<(TranscriptStatus, string), TranscriptStatus> Transitions = new()
        {
            [(TranscriptStatus.BelumDiproses, "MulaiProses")] = TranscriptStatus.SedangDiproses,
            [(TranscriptStatus.SedangDiproses, "GeminiSukses")] = TranscriptStatus.Selesai,
            [(TranscriptStatus.SedangDiproses, "GeminiGagal")] = TranscriptStatus.Gagal,
            [(TranscriptStatus.Gagal, "Retry")] = TranscriptStatus.SedangDiproses,
        };

        public void Apply(string evt)
        {
            if (!Transitions.TryGetValue((Status, evt), out var next))
                throw new InvalidOperationException($"Transcript status '{Status}' tidak bisa diproses dengan event '{evt}'");

            Status = next;
        }
    }
}