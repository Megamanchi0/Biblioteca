using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class DetallePrestamoLibro
    {
        public int Id { get; set; }
        [Required]
        public int IdLibro {  get; set; }
        [Required]
        public int IdPrestamo {  get; set; }
        [JsonIgnore]
        public Libro? Libro { get; set; }
        [JsonIgnore]
        public Prestamo? Prestamo { get; set; }
    }
}
