using API.Usuarios.Models.Entities;

namespace API.Usuarios.Services
{
    public interface ILibroService
    {
        public IEnumerable<Libro> GetAll();
        public DateTime PedirLibro(int dniUsuario, string isbnLibro, DateTime fechaPrestamo);
    }
}
