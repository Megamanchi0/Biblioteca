using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Autor { get; set; }
        [Required]
        public int Anio { get; set; }
        [Required]
        public int IdGenero { get; set; }
        [Required]
        public bool Activo { get; set; }
        [JsonIgnore]
        public Genero? Genero { get; set; }
        [JsonIgnore]
        public ICollection<DetallePrestamoLibro>? DetallePrestamoLibro { get; set; }
    }
}
