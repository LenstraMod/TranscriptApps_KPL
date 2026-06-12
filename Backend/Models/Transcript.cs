namespace Backend.Models
{
    public class Transcript
    {
        public int Id { get; set; }
        public string PsikologScript { get; set; }
        public string PatientScript { get; set; }
        public DateTime createdAt { get; set; }
    }
}
