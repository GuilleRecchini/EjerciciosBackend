using API.Usuarios.Configuration;
using Dapper;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace API.Usuarios.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _conn;

        public UsuarioRepository(IOptions<KeysConfiguration> options)
        {
            _conn = options.Value.ConnectionString;
        }

        public bool Register(Models.Entities.UsuarioDto usuario)
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

        public Models.Entities.UsuarioDto Login(string nombre)
        {
            const string query = "SELECT * FROM Usuarios WHERE nombre = @nombre";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<Models.Entities.UsuarioDto>(query, new { nombre });
            }
        }

        public bool Update(Models.DTOs.UsuarioDto usuario)
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

        public Models.Entities.UsuarioDto GetByEmail(string email)
        {
            const string query = "SELECT * FROM Usuarios WHERE email = @Email";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<Models.Entities.UsuarioDto>(query, new { email });
            }
        }

        public Models.Entities.UsuarioDto GetByDni(int dni)
        {
            const string query = "SELECT * FROM Usuarios WHERE dni = @dni";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.QueryFirstOrDefault<Models.Entities.UsuarioDto>(query, new { dni });
            }
        }

        public void SaveRefreshToken(int userId, string token, DateTime expiryDate, DateTime createdAt)
        {
            const string checkQuery = "SELECT COUNT(1) FROM RefreshTokens WHERE UserId = @UserId";
            const string insertQuery = @"
            INSERT INTO RefreshTokens (UserId, Token, ExpiryDate, CreatedAt)
            VALUES (@UserId, @Token, @ExpiryDate, @CreatedAt)";

            const string updateQuery = @"
            UPDATE RefreshTokens
            SET Token = @Token, ExpiryDate = @ExpiryDate, CreatedAt = @CreatedAt
            WHERE UserId = @UserId";

            using (var connection = new MySqlConnection(_conn))
            {
                var exists = connection.ExecuteScalar<int>(checkQuery, new { UserId = userId });
                if (exists > 0)
                {
                    connection.Execute(updateQuery, new
                    {
                        UserId = userId,
                        Token = token,
                        ExpiryDate = expiryDate,
                        CreatedAt = createdAt
                    });
                }
                else
                {
                    connection.Execute(insertQuery, new
                    {
                        UserId = userId,
                        Token = token,
                        ExpiryDate = expiryDate,
                        CreatedAt = createdAt
                    });
                }
            }
        }

        public bool ValidateRefreshToken(int userId, string token)
        {
            const string query = @"
            SELECT COUNT(1) FROM RefreshTokens 
            WHERE UserId = @UserId AND Token = @Token AND ExpiryDate > NOW()";
            using (var connection = new MySqlConnection(_conn))
            {
                return connection.ExecuteScalar<bool>(query, new { UserId = userId, Token = token });
            }
        }

        public void DeleteRefreshToken(string token)
        {
            const string query = "DELETE FROM RefreshTokens WHERE Token = @Token";
            using (var connection = new MySqlConnection(_conn))
            {
                connection.Execute(query, new { Token = token });
            }
        }

    }
}
