using API.Usuarios.Configuration;
using API.Usuarios.Models.Entities;
using API.Usuarios.Services;
using Dapper;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace API.Usuarios.Data
{
    public class LibroRepository : ILibroRepository
    {
        private readonly string _conn;

        public LibroRepository(IOptions<KeysConfiguration> options)
        {
            _conn = options.Value.ConnectionString;
        }

        public IEnumerable<Libro> GetAll()
        {
            const string query = "SELECT * FROM Libros";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.Query<Libro>(query);
            }
        }

        public bool EstaDisponible(string isbnLibro)
        {
            const string query = "SELECT Disponible FROM Libros WHERE isbn = @isbnLibro";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<bool>(query, new { isbnLibro });
            }
        }

        public DateTime PedirLibro(int dniUsuario, string isbnLibro, DateTime fechaPrestamo, DateTime fechaVencimiento)
        {
            const string query = @"
                INSERT INTO prestamos (dni_usuario, isbn_libro, fecha_prestamo, fecha_vencimiento)
                VALUES (@dniUsuario, @isbnLibro, @fechaPrestamo, @FechaVencimiento);
                
                UPDATE Libros SET Disponible = false WHERE  isbn = @isbnLibro";
            using (var connection = new MySqlConnection(_conn))
            {
                var rowsAffected = connection.Execute(query, new { dniUsuario, isbnLibro, fechaPrestamo, fechaVencimiento });

                if (rowsAffected > 0)
                {
                    return fechaVencimiento;
                }
                else
                {
                    throw new InvalidOperationException("No se pudo registrar el préstamo.");
                }

            }
        }
    }
}
