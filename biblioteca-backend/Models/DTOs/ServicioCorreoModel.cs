namespace biblioteca_backend.Models.DTOs
{
    public class ServicioCorreoModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string html { get; set; }
    }
}
