using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    //Controller untuk autentikasi
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;

        public AuthController(AuthService authService) {
            _authService = authService;
        }

        //Endpoint login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req) 
        {
            var user = _authService.Login(req.Email, req.Password);

            if (user == null) return Unauthorized(new { message = "Email atau password salah" });

            return Ok(new
            {
                message = "Login Berhasil",
                id = user.Id,
                name = user.Name,
                role = user.role,
                email = user.email,
            });
        }

        //Endpoint Register
        [HttpPost("register")]
        public IActionResult Register([FromBody] Patient patient)
        { 
            bool success = _authService.Register(patient);

            if (!success) return Conflict(new { message = "Email sudah terdaftar!"});

            return Ok(new { message = "Registered Berhasil!" });
        }
    }

    //bentuk request untuk login
    public class LoginRequest 
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
