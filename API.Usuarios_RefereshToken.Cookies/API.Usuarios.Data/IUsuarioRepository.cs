using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;

namespace API.Usuarios.Data
{
    public interface IUsuarioRepository
    {
        public bool Register(Models.Entities.UsuarioDto usuario);
        public Models.Entities.UsuarioDto Login(string nombre);
        public bool Update(Models.DTOs.UsuarioDto usuario);
        public Models.Entities.UsuarioDto GetByEmail(string email);
        public Models.Entities.UsuarioDto GetByDni(int dni);
        void SaveRefreshToken(int userId, string token, DateTime expiryDate, DateTime createdAt);
        bool ValidateRefreshToken(int userId, string token);
        void DeleteRefreshToken(string token);
    }
}
