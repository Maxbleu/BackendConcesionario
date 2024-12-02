using Backend_Concesionario.Models;
using Backend_Concesionario.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Concesionario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(TokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User credentials)
        {

            if (credentials.UserName == _configuration["UserAdmin:Name"] && credentials.Password == _configuration["UserAdmin:Password"])
            {
                var token = _tokenService.GenerateJwtToken();
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Credenciales no válidas.");
            }
        }
    }
}
