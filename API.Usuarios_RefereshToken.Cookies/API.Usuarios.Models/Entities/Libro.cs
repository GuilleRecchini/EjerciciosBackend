namespace API.Usuarios.Models.Entities
{
    public class Libro
    {
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public bool Disponible { get; set; }
    }
}
