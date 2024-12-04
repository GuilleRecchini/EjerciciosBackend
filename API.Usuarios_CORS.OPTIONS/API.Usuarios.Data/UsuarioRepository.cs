using API.Usuarios.Configuration;
using API.Usuarios.Models.DTOs;
using API.Usuarios.Models.Entities;
using Dapper;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace API.Usuarios.Data
{
    public class UsuarioRepository
    {
        private static UsuarioRepository _instance;
        private readonly string _conn;

        private UsuarioRepository(IOptions<KeysConfiguration> options)
        {
            _conn = options.Value.ConnectionString;
        }

        public static UsuarioRepository Instance(IOptions<KeysConfiguration> options)
        {
            {
                if (_instance == null)
                {
                    _instance = new UsuarioRepository(options);
                }
                return _instance;
            }
        }

        public bool Register(Usuario usuario)
        {
            const string query = @"
            INSERT INTO Usuarios (dni, nombre, edad, password_hash, email) 
            VALUES (@Dni, @Nombre, @Edad, @Password_Hash, @Email)";

            using (var connection = new MySqlConnection(_conn))
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
            using (var connection = new MySqlConnection(_conn))
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

            using (var connection = new MySqlConnection(_conn))
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
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { email });
            }
        }

        public Usuario GetByDni(int dni)
        {
            const string query = "SELECT * FROM Usuarios WHERE dni = @dni";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { dni });
            }
        }

    }
}
