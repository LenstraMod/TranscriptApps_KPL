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

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromForm] TranscriptReq req)
        {
            if (req.AudioFile == null || req.AudioFile.Length == 0)
            {
                return BadRequest(new { Message = "Audio File Kosong" });
            }

            //Ubah IFormFile jadi byte[]
            using var memoryStream = new MemoryStream();
            await req.AudioFile.CopyToAsync(memoryStream);
            byte[] audioBytes = memoryStream.ToArray();

            var transcript = await _service.GenerateTranscript(
                req.AppointmentId,
                audioBytes
             );

            return Ok(new
            {
                message = "Transcript berhasil dibuat!",
                appointmentId = transcript.appointmentId,
                generatedAt = transcript.createdAt
            });
        }

        [HttpGet("{appointmentId}")]
        public IActionResult GetTranscript(int appointmentIds, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest(new { Message = "Role tidak boleh kosong" });
            if (roles != "Psikolog" && roles != "Patient") return BadRequest(new { Message = "Role tidak Valid!" });

            var transcript = _service.GetTranscript(appointmentIds, roles);

            if (transcript == null) return NotFound(new { Message = "Transcript tidak ditemukan!"});

            return Ok(new { 
                appointmentId = appointmentIds,
                role = roles,
                transcript = transcript
            });
        }

        [HttpGet("{all}")]
        public IActionResult GetAll()
        {
            var transcripts = _service.GetAll();
            return Ok(transcripts);
        }

    }

    public class TranscriptReq 
    { 
        public int AppointmentId { get; set; }
        public IFormFile AudioFile { get; set; }
    }
}
