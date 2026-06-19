using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var schedules = _scheduleService.GetAll();
            return Ok(schedules);
        }

        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            var schedules = _scheduleService.GetAvailable();
            return Ok(schedules);
        }

        [HttpGet("psikolog/{id}")]
        public IActionResult GetByPsikolog(int id)
        {
            var schedules = _scheduleService.GetByPsikolog(id);
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var schedule = _scheduleService.GetById(id);
            if (schedule == null) return NotFound(new { message = "Jadwal tidak ditemukan" });
            return Ok(schedule);
        }

        [HttpPost("book/{id}")]
        public IActionResult BookSchedule(string id, [FromBody] BookedBy bookedBy)
        {
            bool success = _scheduleService.BookSchedule(id, bookedBy);
            if (!success) return BadRequest(new { message = "Jadwal tidak tersedia atau tidak ditemukan" });
            return Ok(new { message = "Booking berhasil" });
        }

        [HttpPost("add")]
        public IActionResult AddSchedule([FromBody] Schedule schedule)
        {
            bool success = _scheduleService.AddSchedule(schedule);
            if (!success) return BadRequest(new { message = "Gagal menambahkan jadwal" });
            return Ok(new { message = "Jadwal berhasil ditambahkan" });
        }

        [HttpPut("status/{id}")]
        public IActionResult UpdateStatus(string id, [FromBody] UpdateStatusRequest req)
        {
            bool success = _scheduleService.ApplyEvent(id, req.Event);
            if (!success) return NotFound(new { message = "Jadwal tidak ditemukan" });
            return Ok(new { message = "Status berhasil diupdate" });
        }

        [HttpPut("{id}")]
        public IActionResult EditSchedule(string id, [FromBody] SessionInfo session)
        {
            bool success = _scheduleService.EditSchedule(id, session);
            if (!success) return BadRequest(new { message = "Jadwal tidak ditemukan atau sudah terbooking" });
            return Ok(new { message = "Jadwal berhasil diupdate" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSchedule(string id)
        {
            bool success = _scheduleService.DeleteSchedule(id);
            if (!success) return BadRequest(new { message = "Jadwal tidak ditemukan atau sudah terbooking" });
            return Ok(new { message = "Jadwal berhasil dihapus" });
        }
    }

    public class UpdateStatusRequest
    {
        public string Event { get; set; }
    }
}