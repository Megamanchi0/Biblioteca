using biblioteca_backend.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblioteca_backend.Clases
{
    public class clsGenero
    {
        private readonly DBBibliotecaContext _biblioteca;
        public clsGenero(DBBibliotecaContext biblioteca)
        {
            _biblioteca = biblioteca;
        }
        public async Task<List<Genero>> ConsultarTodos()
        {
            try
            {
                var list = await _biblioteca.Genero.ToListAsync();
                return list;
            }
            catch (Exception)
            {

                throw new Exception("Error al consultar los géneros");
            }
        }
    }
}
