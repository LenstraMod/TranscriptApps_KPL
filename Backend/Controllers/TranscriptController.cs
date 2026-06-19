using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranscriptController : ControllerBase
    {
        private readonly TranscriptService _service;

        public TranscriptController(TranscriptService service) { 
            _service = service;
        }

        //Merupakan endpoint untuk melakuan generate
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromForm] TranscriptReq req)
        {
            if (req.AudioFile == null || req.AudioFile.Length == 0)
                return BadRequest(new { message = "Audio File Kosong" });

            if (string.IsNullOrWhiteSpace(req.scheduleId))
                return BadRequest(new { message = "scheduleId tidak boleh kosong" });

            // Ubah IFormFile ke byte[] agar bisa dikirim ke GeminiService
            using var memoryStream = new MemoryStream();
            await req.AudioFile.CopyToAsync(memoryStream);
            byte[] audioBytes = memoryStream.ToArray();

            try
            {
                //Mengirim hasil audio ke gemini
                var transcript = await _service.GenerateTranscript(req.scheduleId, audioBytes);

                return Ok(new
                {
                    message = "Transcript berhasil dibuat!",
                    appointmentId = transcript.scheduleId,
                    generatedAt = transcript.createdAt
                });
            }
            catch (InvalidOperationException ex)
            {
                // Terjadi saat jadwal belum Terbooking atau sudah Selesai
                // Kembalikan 400 dengan pesan yang bisa dibaca frontend
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Error lain: Gemini API gagal, file error, dll.
                return StatusCode(500, new { message = "Server error: " + ex.Message });
            }
        }

        // Bug fix: nama parameter route {scheduleId} harus sama dengan nama parameter method
        [HttpGet("{scheduleId}")]
        public IActionResult GetTranscript(string scheduleId, [FromQuery] string role)
        {
            if (string.IsNullOrEmpty(role)) return BadRequest(new { Message = "Role tidak boleh kosong" });
            if (role != "Psikolog" && role != "Patient") return BadRequest(new { Message = "Role tidak Valid!" });

            var transcript = _service.GetTranscript(scheduleId, role);

            if (transcript == null) return NotFound(new { Message = "Transcript tidak ditemukan!" });

            return Ok(new {
                scheduleId = scheduleId,
                role = role,
                transcript = transcript   
            });
        }

        //Ambil semua transcript yang ada
        [HttpGet]
        public IActionResult GetAll()
        {
            var transcripts = _service.GetAll();
            return Ok(transcripts);
        }

    }


    //class untuk request transcript
    public class TranscriptReq 
    { 
        public string scheduleId{ get; set; }
        public IFormFile AudioFile { get; set; }
    }
}
