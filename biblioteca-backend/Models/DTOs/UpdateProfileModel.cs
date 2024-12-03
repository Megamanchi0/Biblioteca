using System.ComponentModel.DataAnnotations;

namespace biblioteca_backend.Models.DTOs
{
    public class UpdateProfileModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
    }
}
