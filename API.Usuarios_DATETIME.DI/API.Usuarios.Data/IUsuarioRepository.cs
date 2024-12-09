using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;

namespace API.Usuarios.Data
{
    public interface IUsuarioRepository
    {
        public bool Register(Usuario usuario);
        public Usuario Login(string nombre);
        public bool Update(UsuarioDto usuario);
        public Usuario GetByEmail(string email);
        public Usuario GetByDni(int dni);
    }
}
