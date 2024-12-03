using biblioteca_backend.Models;

using Microsoft.EntityFrameworkCore;

namespace biblioteca_backend.Clases
{
    public class clsAccion
    {
        private readonly DBBibliotecaContext _biblioteca;
        public clsAccion(DBBibliotecaContext biblioteca)
        {
            _biblioteca = biblioteca;
        }

        public async Task<List<Accion>> ConsultarAcciones()
        {
            try
            {
                var acciones = await _biblioteca.Accion.ToListAsync();
                return acciones;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
