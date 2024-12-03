using biblioteca_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections;

namespace biblioteca_backend.Clases
{
    public class clsLibro
    {
        private readonly DBBibliotecaContext _biblioteca;
        private ILogger<clsLibro> _logger;
        public clsLibro(DBBibliotecaContext biblioteca, ILogger<clsLibro> logger)
        {
            _biblioteca = biblioteca;
            _logger = logger;
        }
        //public IEnumerable ConsultarTodos(int pagina, int numeroRegistros, bool mostrarActivos)
        //{
        //    try
        //    {
        //        var consulta = _biblioteca.PaginarLibros(pagina, numeroRegistros, mostrarActivos);
        //        return consulta;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Error al consultar libros", ex);
        //    }
        //}

        public IEnumerable ConsultarTodos(bool mostrarActivos)
        {
            try
            {
                _logger.LogDebug("Consultando libros...");
                var consulta = (from L in _biblioteca.Libro
                                join G in _biblioteca.Genero
                                on L.IdGenero equals G.Id
                                where (mostrarActivos==true && L.Activo==true) || mostrarActivos == false
                                select new
                                {
                                    id=L.Id,
                                    titulo = L.Titulo,
                                    autor = L.Autor,
                                    anio = L.Anio,
                                    idGenero = L.IdGenero,
                                    genero = G.Nombre,
                                    activo = L.Activo
                                }).ToList();
                _logger.LogInformation("Consulta de libros exitosa");
                return consulta;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar los libros: {mensaje}",ex.Message);
                throw new Exception("Error al consultar libros", ex);
            }
        }

        public async Task<string> Guardar([FromBody] Libro libro)
        {
            try
            {
                _logger.LogDebug("Guardando libro...");
                libro.Activo = true;
                _biblioteca.Add(libro);
                await _biblioteca.SaveChangesAsync();
                _logger.LogInformation("Libro guardado exitosamente");
                return"Libro agregado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al guardar libro: {mensaje}", ex.Message);
                throw new Exception("Error al gaurdar el libro");
            }
        }

        public async Task<string> Actualizar([FromBody] Libro libro)
        {
            try
            {
                _logger.LogDebug("Actualizando libro con id {id}...", libro.Id);
                var _libro = _biblioteca.Libro.FirstOrDefault(e => e.Id == libro.Id);
                if (_libro == null)
                {
                    _logger.LogError("Libro no encontrado");
                    throw new Exception("Libro no encontrado");
                }
                _libro.Titulo = libro.Titulo;
                _libro.Autor = libro.Autor;
                _libro.Anio = libro.Anio;
                _libro.IdGenero = libro.IdGenero;
                _biblioteca.Libro.Update(_libro);
                await _biblioteca.SaveChangesAsync();

                _logger.LogInformation("Libro actualizado con id {id}", _libro.Id);
                return "Registro actualizado correctamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar libro: {mensaje}", ex.Message);
                throw new Exception("Error al actualizar el libro");
    
            }
        }

        public async Task<string> Eliminar(int id)
        {
            try
            {
                _logger.LogDebug("Eliminando libro con id {id}...", id);
                var libro = await _biblioteca.Libro.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("El id {id} no fue encontrado", id);
                    return "";
                }
                if (!libro.Activo)
                {
                    _logger.LogWarning("No se puede eliminar el libro con id {id} porque se encuentra en un préstamo activo", id);
                    throw new Exception("No se puede eliminar el libro porque se encuentra en un préstamo activo");
                }
                _biblioteca.Remove(libro);
                await _biblioteca.SaveChangesAsync();

                _logger.LogInformation("Libro eliminado con id {id}", libro.Id);
                return "Registro eliminado correctamente" ;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al eliminar libro: {mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //public int ContarLibros(bool mostrarActivos)
        //{
        //    try
        //    {
        //        if (mostrarActivos)
        //        {
        //            return _biblioteca.Libro.Where(L => L.Activo).Count();
        //        }

        //        return _biblioteca.Libro.Count();
        //    }
        //    catch (Exception)
        //    {

        //        throw new Exception("Error al traer los datos");
        //    }
            
        //}
    }
}
