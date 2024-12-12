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

        /// <summary>
        /// Este método se encarga de autenticar a un usuario 
        /// en la aplicación devolviendo un token jwt.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] User credentials)
        {
            //  Comprobamos si el usuario recibido es el usuario admin
            if (credentials.UserName == _configuration["UserAdmin:Name"] && credentials.Password == _configuration["UserAdmin:Password"])
            {
                //  Generamos un token JWT y lo devolvemos
                var token = _tokenService.GenerateJwtToken();
                return Ok(token);
            }
            else
            {
                //  Devolver un error 401 Unauthorized
                return Unauthorized("Credenciales no válidas.");
            }
        }
    }
}
