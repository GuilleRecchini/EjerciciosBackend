using API.Usuarios.Models.Entities;
using Dapper;
using MySqlConnector;

namespace API.Usuarios.Data
{
    public class UsuarioRepository
    {
        private static UsuarioRepository _instance;

        private UsuarioRepository()
        {
        }

        public static UsuarioRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UsuarioRepository();
                }
                return _instance;
            }
        }

        const string conn = "Server=127.0.0.1;Database=curso2024;User Id=root;Password=root;";

        public Usuario GetByEmail(string email)
        {
            const string query = "SELECT * FROM Usuarios WHERE email = @Email";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { email });
            }
        }

        public Usuario GetByDni(int dni)
        {
            const string query = "SELECT * FROM Usuarios WHERE dni = @dni";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { dni });
            }
        }

        public bool Insert(Usuario usuario)
        {
            const string query = @"
            INSERT INTO Usuarios (dni, nombre, edad, email) 
            VALUES (@Dni, @Nombre, @Edad, @Email)";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new
                {
                    usuario.Dni,
                    usuario.Nombre,
                    usuario.Edad,
                    usuario.Email
                });

                return rowsAffected > 0;
            }
        }

        public bool Update(Usuario usuario)
        {
            const string query = @"
            UPDATE Usuarios 
            SET nombre = @Nombre, edad = @Edad, email = @Email
            WHERE dni = @Dni";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new
                {
                    usuario.Dni,
                    usuario.Nombre,
                    usuario.Edad,
                    usuario.Email
                });

                return rowsAffected > 0;
            }
        }
    }
}
