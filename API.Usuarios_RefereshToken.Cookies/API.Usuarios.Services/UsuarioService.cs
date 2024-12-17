using API.Usuarios.Configuration;
using API.Usuarios.Data;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Services.Utils;
using Microsoft.Extensions.Options;

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

            var usuario = new Models.Entities.UsuarioDto
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

        public Models.DTOs.UsuarioDto Login(LoginDto loginDto)
        {
            var usuario = _usuarioRepository.Login(loginDto.Nombre);

            var isPasswordValid = PasswordHasher.VerifyPassword(loginDto.Password, usuario.Password_Hash);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("El usuario o la contraseña son incorrectos. Por favor, inténtelo nuevamente.");

            return new Models.DTOs.UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
        }

        public string Update(Models.DTOs.UsuarioDto usuario)
        {
            if (usuario.Edad <= 14)
                return "La edad debe ser mayor a 14";

            if (!usuario.Email.Contains("@gmail"))
                return "El email debe ser Gmail";

            var result = _usuarioRepository.Update(usuario);

            return result ? "Usuario modificado" : "Error al modificar el usuario";

        }

        public Models.DTOs.UsuarioDto GetByDni(int dni)
        {
            var usuario = _usuarioRepository.GetByDni(dni);

            return new Models.DTOs.UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
        }
        public Models.DTOs.UsuarioDto GetByEmail(string email)
        {
            var usuario = _usuarioRepository.GetByEmail(email);

            return new Models.DTOs.UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
        }

        public void SaveRefreshToken(int userId, string token)
        {
            var expiryDate = DateTime.UtcNow.AddDays(7);
            var createdAt = DateTime.UtcNow;

            _usuarioRepository.SaveRefreshToken(userId, token, expiryDate, createdAt);
        }

        public bool ValidateRefreshToken(int userId, string token)
        {
            return _usuarioRepository.ValidateRefreshToken(userId, token);
        }

        public void DeleteRefreshToken(string token)
        {
            _usuarioRepository.DeleteRefreshToken(token);
        }
    }
}
