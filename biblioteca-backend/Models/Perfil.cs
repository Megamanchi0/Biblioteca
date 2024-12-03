using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Perfil
    {
        public int Id { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Contrasena { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public int IdRol { get; set; }
        [JsonIgnore]
        public Rol? Rol { get; set; }
        [JsonIgnore]
        public ICollection<Prestamo>? Prestamos { get; set; }
        [JsonIgnore]
        public ICollection<RegistroArchivoPerfil>? RegistroArchivosPerfil { get; set; }

    }
}
