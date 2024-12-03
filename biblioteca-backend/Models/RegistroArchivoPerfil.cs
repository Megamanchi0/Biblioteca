using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class RegistroArchivoPerfil
    {
        public int Id { get; set; }
        [Required]
        public int IdPerfil { get; set; }
        [Required]
        public int IdArchivo { get; set; }
        [Required]
        public int IdAccion { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        [JsonIgnore]
        public Perfil? Perfil { get; set; }
        [JsonIgnore]
        public Archivo? Archivo { get; set; }
        [JsonIgnore]
        public Accion? Accion { get; set; }
    }
}
