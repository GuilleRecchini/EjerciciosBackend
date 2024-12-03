using API.Usuarios.Data;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;
using API.Usuarios.Services.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/*
 crear una API con siguientes endpoints:
-registro de usuario, que guarde el usuario con la contraseña hasheada,
-login de usuario, que valida pass y contraseña y devuelve un token JWT junto con la informaicon del usuario (en un objeto)
-endpoint para obtener la informacion de un usuario cualquiera, obtenido por atributo, este endpoint solo debe poder ser llamado 
 por un usuario logueado. 
 */

namespace API.Usuarios.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _usuarioRepository = UsuarioRepository.Instance;
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

            var llaveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_secret_secret_secret_secret"));
            var credencialesFirma = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new("Usuario",loginDto.Nombre),
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
