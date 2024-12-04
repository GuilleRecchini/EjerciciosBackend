using API.Usuarios.Models.DTOs;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Usuarios.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize(Roles = "usuario")]
        [HttpGet("GetByDni/{dni}")]
        public UsuarioDto GetByDni(int dni)
        {
            return _usuarioService.GetByDni(dni);
        }

        [Authorize(Roles = "usuario")]
        [HttpGet("GetByEmail/{email}")]
        public UsuarioDto GetByEmail(string email)
        {
            return _usuarioService.GetByEmail(email);
        }

        [HttpPost("Registro")]
        public string Register([FromBody] RegisterDto registerDto)
        {
            return _usuarioService.Register(registerDto);
        }

        [HttpPost("Login")]
        public object Login([FromBody] LoginDto loginDto)
        {
            return _usuarioService.Login(loginDto);
        }

        [HttpPut("Update")]
        public string Update(UsuarioDto usuario)
        {
            return _usuarioService.Update(usuario);
        }
    }
}
