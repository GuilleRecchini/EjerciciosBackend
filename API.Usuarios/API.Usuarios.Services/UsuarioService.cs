using API.Usuarios.Data;
using API.Usuarios.Models.Entities;

namespace API.Usuarios.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _usuarioRepository = UsuarioRepository.Instance;
        }
        public Usuario GetByDni(int dni)
        {
            return _usuarioRepository.GetByDni(dni);
        }
        public Usuario GetByEmail(string email)
        {
            return _usuarioRepository.GetByEmail(email);
        }

        public string Insert(Usuario usuario)
        {
            if (usuario.Edad <= 14)
                return "La edad debe ser mayor a 14";

            if (!usuario.Email.Contains("@gmail"))
                return "El email debe ser Gmail";

            var result = _usuarioRepository.Insert(usuario);

            return result ? "Usuario creado" : "Error al crear el usuario";
        }

        public string Update(Usuario usuario)
        {
            if (usuario.Edad <= 14)
                return "La edad debe ser mayor a 14";

            if (!usuario.Email.Contains("@gmail"))
                return "El email debe ser Gmail";

            var result = _usuarioRepository.Update(usuario);

            return result ? "Usuario modificado" : "Error al modificar el usuario";

        }

    }
}
