using System.Text.Json.Serialization;

namespace biblioteca_backend.Models
{
    public class Accion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        [JsonIgnore]
        public ICollection<RegistroArchivoPerfil>? RegistroArchivosPerfil { get; set; }
    }
}
