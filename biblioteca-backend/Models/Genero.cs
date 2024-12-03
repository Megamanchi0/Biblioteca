using System.ComponentModel.DataAnnotations;

namespace biblioteca_backend.Models
{
    public class Genero
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public ICollection<Libro> Libros { get; set; }
    }
}
