using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Rol
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [JsonIgnore]
        public ICollection<Perfil>? Perfiles { get; set; }
    }
}
