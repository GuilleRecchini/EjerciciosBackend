using API.Usuarios.Data;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;
using API.Usuarios.Services.Utils;

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

        public UsuarioDto Login(LoginDto loginDto)
        {
            var usuario = _usuarioRepository.Login(loginDto.Nombre);

            var isPasswordValid = PasswordHasher.VerifyPassword(loginDto.Password, usuario.Password_Hash);

            if (!isPasswordValid)
                return new UsuarioDto();

            return new UsuarioDto
            {
                Dni = usuario.Dni,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Edad = usuario.Edad
            };
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
