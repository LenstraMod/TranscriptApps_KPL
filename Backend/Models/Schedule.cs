using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Schedule
    {
        [JsonPropertyName("scheduleId")]
        public string ScheduleId { get; set; }

        [JsonPropertyName("psychologist")]
        public PsikologInfo Psychologist { get; set; }

        [JsonPropertyName("session")]
        public SessionInfo Session { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("bookedBy")]
        public BookedBy? BookedBy { get; set; }
    }

    public class PsikologInfo
    {
        [JsonPropertyName("psychologistId")]
        public int PsychologistId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("strNumber")]
        public string StrNumber { get; set; }

        [JsonPropertyName("experience")]
        public int Experience { get; set; }
    }

    public class SessionInfo
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("meetingLink")]
        public string? MeetingLink { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }
    }

    public class BookedBy
    {
        [JsonPropertyName("patientId")]
        public int PatientId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("contact")]
        public ContactInfo Contact { get; set; }

        [JsonPropertyName("complaint")]
        public string Complaint { get; set; }

        [JsonPropertyName("sessionHistory")]
        public List<SessionHistory> SessionHistory { get; set; } = new();
    }

    public class ContactInfo
    {
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class SessionHistory
    {
        [JsonPropertyName("scheduleId")]
        public string ScheduleId { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }
}