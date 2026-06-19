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
            // Precondition
            Contract.Requires(psikologId > 0, "PsikologId harus lebih dari 0");

            return _scheduleList.Where(s => s.Psychologist.PsychologistId == psikologId).ToList();
        }

        public Schedule? GetById(string scheduleId)
        {
            // Precondition
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");

            return _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
        }

        public bool BookSchedule(string scheduleId, BookedBy bookedBy)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");
            Contract.Requires(bookedBy != null, "Data bookedBy tidak boleh null");

            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            try { schedule.Apply("Booking"); }
            catch (InvalidOperationException) { return false; }

            schedule.BookedBy = bookedBy;
            SaveData();

            // Postcondition: status jadwal harus berubah ke Terbooking
            Contract.Ensures(schedule.Status == ScheduleStatus.Terbooking,
                "Status jadwal harus Terbooking setelah booking berhasil");

            return true;
        }

        //evt = nama event, divalidasi lewat Schedule.Apply()
        public bool ApplyEvent(string scheduleId, string evt)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");
            Contract.Requires(!string.IsNullOrWhiteSpace(evt), "Nama event tidak boleh kosong");

            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            try { schedule.Apply(evt); }
            catch (InvalidOperationException) { return false; }

            SaveData();
            return true;
        }

        public bool AddSchedule(Schedule newSchedule)
        {
            // Preconditions
            Contract.Requires(newSchedule != null, "Schedule tidak boleh null");
            Contract.Requires(newSchedule!.Psychologist != null, "Data psikolog pada schedule tidak boleh null");
            Contract.Requires(newSchedule.Session != null, "Data sesi pada schedule tidak boleh null");

            int countBefore = _scheduleList.Count;

            newSchedule.ScheduleId = GenerateNextId();
            newSchedule.Status = ScheduleStatus.Terbooking;
            _scheduleList.Add(newSchedule);
            SaveData();

            // Postcondition: list harus bertambah satu entry
            Contract.Ensures(_scheduleList.Count == countBefore + 1,
                "Jumlah jadwal harus bertambah 1 setelah AddSchedule");

            return true;
        }

        public bool EditSchedule(string scheduleId, SessionInfo updatedSession)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");
            Contract.Requires(updatedSession != null, "Data sesi yang diupdate tidak boleh null");

            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            schedule.Session = updatedSession;
            SaveData();

            // Postcondition: session pada jadwal harus sudah terupdate
            Contract.Ensures(schedule.Session == updatedSession,
                "Session pada jadwal harus terupdate setelah EditSchedule");

            return true;
        }

        public bool DeleteSchedule(string scheduleId)
        {
            // Precondition
            Contract.Requires(!string.IsNullOrWhiteSpace(scheduleId), "ScheduleId tidak boleh kosong");

            var schedule = _scheduleList.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null) return false;

            int countBefore = _scheduleList.Count;
            _scheduleList.Remove(schedule);
            SaveData();

            // Postcondition: list harus berkurang satu dan ID tidak boleh ada lagi
            Contract.Ensures(_scheduleList.Count == countBefore - 1,
                "Jumlah jadwal harus berkurang 1 setelah DeleteSchedule");
            Contract.Ensures(!_scheduleList.Any(s => s.ScheduleId == scheduleId),
                "ScheduleId yang dihapus tidak boleh ada lagi di list");

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