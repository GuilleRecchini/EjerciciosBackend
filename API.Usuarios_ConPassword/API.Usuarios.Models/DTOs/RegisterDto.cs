namespace API.Usuarios.Models.DTOs
{
    public class RegisterDto
    {
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Edad { get; set; }
    }
}
