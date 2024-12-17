using API.Usuarios.Configuration;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace API.Usuarios.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _usuarioService;
        private readonly KeysConfiguration _keysConfiguration;

        public UsuarioController(IUsuarioService usuarioService, IOptions<KeysConfiguration> options)
        {
            _usuarioService = usuarioService;
            _keysConfiguration = options.Value;
        }

        [Authorize(Roles = "usuario")]
        [HttpGet("GetByDni/{dni}")]
        public Models.DTOs.UsuarioDto GetByDni(int dni)
        {
            return _usuarioService.GetByDni(dni);
        }

        [Authorize(Roles = "usuario")]
        [HttpGet("GetByEmail/{email}")]
        public Models.DTOs.UsuarioDto GetByEmail(string email)
        {
            return _usuarioService.GetByEmail(email);
        }

        [HttpPost("Registro")]
        public string Register([FromBody] RegisterDto registerDto)
        {
            return _usuarioService.Register(registerDto);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var usuario = _usuarioService.Login(loginDto);
                var accessToken = GenerateJwtToken(usuario);
                var refreshToken = GenerateRefreshToken();

                _usuarioService.SaveRefreshToken(usuario.Dni, refreshToken);

                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(1200),
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                });

                return Ok(new
                {
                    Token = accessToken,
                    Usuario = usuario
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh Token no encontrado");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var userId = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "Dni").Value);

            var isValid = _usuarioService.ValidateRefreshToken(userId, refreshToken);

            if (!isValid)
                return Unauthorized();

            var userDto = _usuarioService.GetByDni(userId);
            var newAccessToken = GenerateJwtToken(userDto);
            var newRefreshToken = GenerateRefreshToken();

            _usuarioService.SaveRefreshToken(userDto.Dni, newRefreshToken);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(1200),
                HttpOnly = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new
            {
                AccessToken = newAccessToken,
            });
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh Token no encontrado");

            _usuarioService.DeleteRefreshToken(refreshToken);

            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Logout exitoso" });
        }


        [HttpPut("Update")]
        public string Update(UsuarioDto usuario)
        {
            return _usuarioService.Update(usuario);
        }

        private string GenerateJwtToken(UsuarioDto usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keysConfiguration.JWT));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new("Usuario",usuario.Nombre),
                new("Dni",usuario.Dni.ToString()),
                new(ClaimTypes.Role, "usuario")
            };
            var token = new JwtSecurityToken(
                "http://localhost/",
                "http://localhost/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
