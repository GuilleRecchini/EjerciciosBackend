using API.Usuarios.Configuration;
using API.Usuarios.Models.Entities;
using Microsoft.Extensions.Options;

namespace API.Usuarios.Services
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly KeysConfiguration _keysConfiguration;

        public LibroService(IOptions<KeysConfiguration> options, ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
            _keysConfiguration = options.Value;
        }

        public IEnumerable<Libro> GetAll()
        {
            return _libroRepository.GetAll();
        }

        public DateTime PedirLibro(int dniUsuario, string isbnLibro, DateTime fechaPrestamo)
        {
            if (!_libroRepository.EstaDisponible(isbnLibro))
                throw new InvalidOperationException("El libro no está disponible.");

            var fechaVencimiento = fechaPrestamo.AddDays(5);

            return _libroRepository.PedirLibro(dniUsuario, isbnLibro, fechaPrestamo, fechaVencimiento);
        }
    }
}
