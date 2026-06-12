namespace Backend.Models
{
    public class AudioSession
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string AudioPath { get; set; }
        public DateTime createdAt { get; set; }
        public string Status { get; set; }
    }
}
