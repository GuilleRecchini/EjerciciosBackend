namespace API.Usuarios.Models.DTOs
{
    public class PedirLibroRequest
    {
        public int DniUsuario { get; set; }
        public string IsbnLibro { get; set; }
        public DateTime FechaPrestamo { get; set; }
    }
}
