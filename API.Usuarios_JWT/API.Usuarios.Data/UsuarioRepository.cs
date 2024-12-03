using API.Usuarios.Models.DTOs;
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

        public bool Register(Usuario usuario)
        {
            const string query = @"
            INSERT INTO Usuarios (dni, nombre, edad, password_hash, email) 
            VALUES (@Dni, @Nombre, @Edad, @Password_Hash, @Email)";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new
                {
                    usuario.Dni,
                    usuario.Nombre,
                    usuario.Edad,
                    usuario.Password_Hash,
                    usuario.Email
                });

                return rowsAffected > 0;
            }
        }

        public Usuario Login(string nombre)
        {
            const string query = "SELECT * FROM Usuarios WHERE nombre = @nombre";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { nombre });
            }
        }

        public bool Update(UsuarioDto usuario)
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

    }
}
