using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Archivo
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaCarga { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        [Required]
        public int NumeroDescargas { get; set; }
        [Required]
        public string RutaDocumento { get; set; }

        [JsonIgnore]
        public ICollection<RegistroArchivoPerfil>? RegistroArchivosPerfil { get; set; }
    }
}
