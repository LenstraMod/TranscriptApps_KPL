namespace Backend.Models
{
    public class Transcript
    {
        public int id { get; set; }
        public int appointmentId { get; set; }
        public string technicalTranscript { get; set; }
        public string simplifiedTranscripts { get; set; }
        public DateTime createdAt { get; set; }
    }
}
