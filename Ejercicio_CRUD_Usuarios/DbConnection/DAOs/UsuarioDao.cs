using Dapper;
using DbConnection.Entities;
using MySqlConnector;

namespace DbConnection.DAOs
{
    public class UsuarioDao
    {
        const string conn = "Server=127.0.0.1;Database=curso2024;User Id=root;Password=root;";
        public Usuario GetUsuario(int dni)
        {
            const string query = "SELECT * FROM Usuarios WHERE dni = @dni";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { dni });
            }
        }

        public IEnumerable<Usuario> GetUsuarios()
        {
            const string query = "SELECT * FROM Usuarios";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.Query<Usuario>(query);
            }
        }

        public IEnumerable<Usuario> GetUsuariosActivos()
        {
            const string query = "SELECT * FROM Usuarios WHERE Eliminado = 0";
            using (var connection = new MySqlConnection(conn))
            {
                return connection.Query<Usuario>(query);
            }
        }

        public bool InsertUsuario(Usuario usuario)
        {
            const string query = @"
            INSERT INTO Usuarios (dni, nombre, edad) 
            VALUES (@Dni, @Nombre, @Edad)";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new
                {
                    usuario.Dni,
                    usuario.Nombre,
                    usuario.Edad
                });

                return rowsAffected > 0;
            }
        }

        public bool UpdateUsuario(Usuario usuario)
        {
            const string query = @"
            UPDATE Usuarios 
            SET nombre = @Nombre, edad = @Edad
            WHERE dni = @Dni";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new
                {
                    usuario.Dni,
                    usuario.Nombre,
                    usuario.Edad
                });

                return rowsAffected > 0;
            }
        }

        public bool DeleteUsuario(int dni)
        {
            const string query = "UPDATE Usuarios SET Eliminado = 1 WHERE dni = @Dni";

            using (var connection = new MySqlConnection(conn))
            {
                var rowsAffected = connection.Execute(query, new { Dni = dni });

                return rowsAffected > 0;
            }
        }
    }
}
