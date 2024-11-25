using API.Usuarios.Models.Entities;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Usuarios.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioService _usuarioService;

        public UsuarioController()
        {
            _usuarioService = new UsuarioService();
        }

        [HttpGet("GetByDni/{dni}")]
        public Usuario GetByDni(int dni)
        {
            return _usuarioService.GetByDni(dni);
        }

        [HttpGet("GetByEmail/{email}")]
        public Usuario GetByDni(string email)
        {
            return _usuarioService.GetByEmail(email);
        }

        [HttpPost]
        public string Insert(Usuario usuario)
        {
            return _usuarioService.Insert(usuario);
        }

        [HttpPut]
        public string Update(Usuario usuario)
        {
            return _usuarioService.Update(usuario);
        }
    }
}
