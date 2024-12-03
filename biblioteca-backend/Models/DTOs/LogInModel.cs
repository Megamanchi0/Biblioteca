using System.ComponentModel.DataAnnotations;

namespace biblioteca_backend.Models.DTOs
{
    public class LogInModel
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}
