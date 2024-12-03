namespace biblioteca_backend.Models.DTOs
{
    public class ConsultaPrestamoModel
    {
        public List<Libro> Libros { get; set; }
        public UpdateProfileModel DatosPerfil { get; set; }
        public DateTime FechaPrestamo {  get; set; }
    }
}
