using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        [Required]
        public DateTime FechaPrestamo { get; set; }
        [Required]
        public int IdPerfil { get; set; }
        [JsonIgnore]
        public Perfil? Perfil { get; set; }
        public ICollection<DetallePrestamoLibro> DetallePrestamoLibro { get; set; }
    }
}
