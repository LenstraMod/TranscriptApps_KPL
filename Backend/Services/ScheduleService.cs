using Backend.Models;
using JsonHelperLibrary;

namespace Backend.Services
{
    //Service yang menangani sesi jadwal
    public class ScheduleService
    {
        private readonly JsonHelper _jsonHelper;
        private List<Schedule> _scheduleList;

        public ScheduleService(JsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
            _scheduleList = _jsonHelper.LoadJson<Schedule>("schedule.json");
        }

        //Mengambil semua list jadwal
        public List<Schedule> GetAll() => _scheduleList;

        //Mengambil semua jadwal yang available
        public List<Schedule> GetAvailable()
        {
            return _scheduleList.Where(s => s.Status == ScheduleStatus.Tersedia).ToList();
        }

        
        public List<Schedule> GetByPsikolog(int psikologId)
        {
            return _scheduleList.Where(s => s.Psychologist.PsychologistId == psikologId).ToList();
        }

        public Schedule? GetById(string scheduleId)
        {
            return _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
        }

        public bool BookSchedule(string scheduleId, BookedBy bookedBy)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            try { schedule.Apply("Booking"); }
            catch (InvalidOperationException) { return false; }

            schedule.BookedBy = bookedBy;
            SaveData();
            return true;
        }

        //evt = nama event, divalidasi lewat Schedule.Apply()
        public bool ApplyEvent(string scheduleId, string evt)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            try { schedule.Apply(evt); }
            catch (InvalidOperationException) { return false; }

            SaveData();
            return true;
        }

        public bool AddSchedule(Schedule newSchedule)
        {
            newSchedule.ScheduleId = GenerateNextId();
            newSchedule.Status = ScheduleStatus.Terbooking;
            _scheduleList.Add(newSchedule);
            SaveData();
            return true;
        }

        public bool EditSchedule(string scheduleId, SessionInfo updatedSession)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);

            schedule.Session = updatedSession;
            SaveData();
            return true;
        }

        public bool DeleteSchedule(string scheduleId)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            _scheduleList.Remove(schedule);
            SaveData();
            return true;
        }

        private void SaveData() => _jsonHelper.SaveJson("schedule.json", _scheduleList);

        private string GenerateNextId()
        {
            int max = _scheduleList.Count > 0
                ? _scheduleList.Max(s => int.Parse(s.ScheduleId.Replace("SCH-", "")))
                : 0;
            return $"SCH-{(max + 1):D3}";
        }
    }
}