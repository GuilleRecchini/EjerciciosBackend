using API.Usuarios.Models.DTOs;

namespace API.Usuarios.Services
{
    public interface IUsuarioService
    {
        public string Register(RegisterDto registerDto);
        public object Login(LoginDto loginDto);
        public string Update(UsuarioDto usuario);
        public UsuarioDto GetByDni(int dni);
        public UsuarioDto GetByEmail(string email);
    }
}
