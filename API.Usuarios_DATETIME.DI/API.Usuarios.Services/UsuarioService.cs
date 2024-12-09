using API.Usuarios.Configuration;
using API.Usuarios.Data;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;
using API.Usuarios.Services.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Usuarios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly KeysConfiguration _keysConfiguration;

        public UsuarioService(IOptions<KeysConfiguration> options, IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _keysConfiguration = options.Value;
        }

        public string Register(RegisterDto registerDto)
        {
            if (registerDto.Edad <= 14)
                return "La edad debe ser mayor a 14";

            if (!registerDto.Email.Contains("@gmail"))
                return "El email debe ser Gmail";

            var hashedPassword = PasswordHasher.HashPassword(registerDto.Password);

            var usuario = new Usuario
            {
                Dni = registerDto.Dni,
                Nombre = registerDto.Nombre,
                Email = registerDto.Email,
                Password_Hash = hashedPassword,
                Edad = registerDto.Edad
            };

            var registrationResult = _usuarioRepository.Register(usuario);

            return registrationResult ? "Usuario creado" : "Error al crear el usuario";
        }

        public object Login(LoginDto loginDto)
        {
            var usuario = _usuarioRepository.Login(loginDto.Nombre);

            var isPasswordValid = PasswordHasher.VerifyPassword(loginDto.Password, usuario.Password_Hash);

            if (!isPasswordValid)
                return "El usuario o la contraseña son incorrectos. Por favor, inténtelo nuevamente.";

            var llaveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keysConfiguration.JWT));
            var credencialesFirma = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new("Usuario",usuario.Nombre),
                new("Dni",usuario.Dni.ToString()),
                new(ClaimTypes.Role, "usuario")
            };
            var tokenSeguridad = new JwtSecurityToken(
                "http://localhost/",
                "http://localhost/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credencialesFirma);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            return new
            {
                Token = token,
                Usuario = new UsuarioDto()
                {
                    Dni = usuario.Dni,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Edad = usuario.Edad
                }
            }; ;
        }

        public string Update(UsuarioDto usuario)
        {
            if (usuario.Edad <= 14)
                return "La edad debe ser mayor a 14";

            if (!usuario.Email.Contains("@gmail"))
                return "El email debe ser Gmail";

            var result = _usuarioRepository.Update(usuario);

            return result ? "Usuario modificado" : "Error al modificar el usuario";

        }

        public UsuarioDto GetByDni(int dni)
        {
            var usuario = _usuarioRepository.GetByDni(dni);

            return new UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
        }
        public UsuarioDto GetByEmail(string email)
        {
            var usuario = _usuarioRepository.GetByEmail(email);

            return new UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
        }

    }
}
