namespace API.Usuarios.Models.Entities
{
    public class UsuarioDto
    {
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password_Hash { get; set; }
        public int Edad { get; set; }
    }
}
