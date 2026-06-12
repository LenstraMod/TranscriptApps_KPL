namespace Backend.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PsikologId { get; set; }
        public DateTime Schedule { get; set; }
        public string Status { get; set; }
    }
}
