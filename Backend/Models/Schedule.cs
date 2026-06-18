namespace Backend.Models
{
    public class Schedule
    {
        public string ScheduleId { get; set; }
        public PsikologInfo Psychologist { get; set; }
        public SessionInfo Session { get; set; }
        public string Status { get; set; }
        public BookedBy? BookedBy { get; set; }
    }

    public class PsikologInfo
    {
        public int PsychologistId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string StrNumber { get; set; }
        public int Experience { get; set; }
    }

    public class SessionInfo
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Method { get; set; }
        public string? MeetingLink { get; set; }
        public string? Location { get; set; }
    }

    public class BookedBy
    {
        public int PatientId { get; set; }
        public string? Name { get; set; }
        public ContactInfo Contact { get; set; }
        public string Complaint { get; set; }
        public List<SessionHistory> SessionHistory { get; set; } = new();
    }

    public class ContactInfo
    {
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class SessionHistory
    {
        public string ScheduleId { get; set; }
        public string Date { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}