using API.Usuarios.Models.Entities;

namespace API.Usuarios.Services
{
    public interface ILibroRepository
    {
        public IEnumerable<Libro> GetAll();
        public bool EstaDisponible(string isbnLibro);
        public DateTime PedirLibro(int dniUsuario, string isbnLibro, DateTime fechaPrestamo, DateTime fechaVencimiento);
    }
}
