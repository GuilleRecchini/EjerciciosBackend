using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Usuarios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibroController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        [Authorize(Roles = "usuario")]
        [HttpGet("GetAll")]
        public IEnumerable<Libro> GetAll()
        {
            return _libroService.GetAll();
        }

        [Authorize(Roles = "usuario")]
        [HttpPut("PedirLibro")]
        public IActionResult PedirLibro([FromBody] PedirLibroRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var dniClaim = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "Dni").Value);

            if (!dniClaim.Equals(request.DniUsuario))
                return BadRequest("El usuario solicitante no coincide con el usuario del préstamo.");

            try
            {
                var fechaDevolucion = _libroService.PedirLibro(dniClaim, request.IsbnLibro, request.FechaPrestamo);
                return Ok(new { FechaDevolucion = fechaDevolucion });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado. Por favor intente nuevamente.");
            }
        }
    }
}
