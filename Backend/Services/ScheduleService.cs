using Backend.Models;
using JsonHelperLibrary;

namespace Backend.Services
{
    public class ScheduleService
    {
        private readonly JsonHelper _jsonHelper;
        private List<Schedule> _scheduleList;

        public ScheduleService(JsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
            _scheduleList = _jsonHelper.LoadJson<Schedule>("schedule.json");
        }

        public List<Schedule> GetAll()
        {
            return _scheduleList;
        }

        public List<Schedule> GetAvailable()
        {
            return _scheduleList.Where(s => s.Status == "tersedia").ToList();
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
            if (schedule == null || schedule.Status != "tersedia") return false;

            schedule.Status = "terbooking";
            schedule.BookedBy = bookedBy;

            SaveData();
            return true;
        }

        public bool UpdateStatus(string scheduleId, string newStatus)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            schedule.Status = newStatus;
            SaveData();
            return true;
        }

        public bool AddSchedule(Schedule newSchedule)
        {
            newSchedule.ScheduleId = GenerateNextId();
            _scheduleList.Add(newSchedule);
            SaveData();
            return true;
        }

        public bool DeleteSchedule(string scheduleId)
        {
            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null || schedule.Status != "tersedia") return false;

            _scheduleList.Remove(schedule);
            SaveData();
            return true;
        }

        private void SaveData()
        {
            _jsonHelper.SaveJson("schedule.json", _scheduleList);
        }

        private string GenerateNextId()
        {
            int max = _scheduleList.Count > 0
                ? _scheduleList.Max(s => int.Parse(s.ScheduleId.Replace("SCH-", "")))
                : 0;
            return $"SCH-{(max + 1):D3}";
        }
    }
}