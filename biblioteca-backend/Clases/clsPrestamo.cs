using biblioteca_backend.Controllers;
using biblioteca_backend.Models;
using biblioteca_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace biblioteca_backend.Clases
{
    public class clsPrestamo
    {
        private readonly DBBibliotecaContext _biblioteca;
        private ILogger<clsPrestamo> _logger;
        public clsPrestamo(DBBibliotecaContext biblioteca, ILogger<clsPrestamo> logger)
        {
            _biblioteca = biblioteca;
            _logger = logger;
        }

        public async Task<string> GuardarPrestamo(PrestamoModel datosPrestamo)
        {
            try
            {
                _logger.LogDebug("Guardando préstamo...");
                var consulta = ConsultarLibrosPrestamo(datosPrestamo.IdPerfil);

                if (consulta.Count>0)
                {
                    _logger.LogWarning("Aún hay préstamos activos del perfil con id {id}", datosPrestamo.IdPerfil);
                    throw new Exception("Aún tiene préstamos activos");
                }

                List<Libro> libros = datosPrestamo.Libros;
                Prestamo prestamo = new Prestamo();
                prestamo.FechaPrestamo = DateTime.Now;
                prestamo.IdPerfil = datosPrestamo.IdPerfil;

                await _biblioteca.Prestamo.AddAsync(prestamo);
                await _biblioteca.SaveChangesAsync();

                foreach (var libro in libros)
                {
                    if (libro.Activo)
                    {
                        DetallePrestamoLibro detallePrestamo = new DetallePrestamoLibro();
                        detallePrestamo.IdPrestamo = prestamo.Id;
                        detallePrestamo.IdLibro = libro.Id;
                        await _biblioteca.DetallePrestamoLibro.AddAsync(detallePrestamo);
                        await _biblioteca.SaveChangesAsync();

                        libro.Activo = false;
                        _biblioteca.Libro.Update(libro);
                        _biblioteca.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No se puede hacer el préstamo de los libros solicitados");
                    }

                }

                _logger.LogInformation("Préstamo realizado exitosamente");
                return "Préstamo realizado exitosamente";

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al hacer el préstamo: {mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> EliminarPrestamo(int idPerfil)
        {
            try
            {
                _logger.LogDebug("Eliminando préstamo del perfil con id {id}", idPerfil);
                List<Libro> listaLibros = ConsultarLibrosPrestamo(idPerfil);
                var prestamo = await _biblioteca.Prestamo.FirstOrDefaultAsync(p => p.IdPerfil == idPerfil);

                var detallePrestamo = await _biblioteca.DetallePrestamoLibro.Where(d => d.IdPrestamo == prestamo!.Id).ToListAsync();

                _biblioteca.DetallePrestamoLibro.RemoveRange(detallePrestamo);
                await _biblioteca.SaveChangesAsync();

                _biblioteca.Prestamo.Remove(prestamo!);
                await _biblioteca.SaveChangesAsync();

                foreach (var libro in listaLibros)
                {
                    libro.Activo = true;
                    _biblioteca.Libro.Update(libro);
                    _biblioteca.SaveChanges();
                }

                _logger.LogInformation("Se eliminó el préstamo del perfil con id {id}", idPerfil);
                return "Se ha devuelto el préstamo exitosamente";

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al eliminar el préstamo: {mensaje}", ex.Message);
                throw new Exception("Error al devolver el préstamo");
            }
        }

        public List<Libro> ConsultarLibrosPrestamo(int idPerfil)
        {
            try
            {
                _logger.LogDebug("Consultando libros del préstamo del perfil con id {id}", idPerfil);
                var consulta = from P in _biblioteca.Perfil
                               join PR in _biblioteca.Prestamo
                               on P.Id equals PR.IdPerfil
                               join DP in _biblioteca.DetallePrestamoLibro
                               on PR.Id equals DP.IdPrestamo
                               join L in _biblioteca.Libro
                               on DP.IdLibro equals L.Id
                               where P.Id == idPerfil
                               select new Libro()
                               {
                                   Id=L.Id,
                                   Titulo=L.Titulo,
                                   Autor=L.Autor,
                                   IdGenero = L.IdGenero,
                                   Anio = L.Anio,
                                   Activo=L.Activo
                               };

                _logger.LogInformation("Libros encontrados exitosamente");
                return consulta.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar libros en préstamo: {mensaje}", ex.Message);
                throw new Exception("Error al consultar préstamo");
            }
        }

        public async Task<List<ConsultaPrestamoModel>> ConsultarPrestamos()
        {
            try
            {
                List<ConsultaPrestamoModel> modeloPrestamos = new List<ConsultaPrestamoModel> ();
                var prestamos = await _biblioteca.Prestamo.ToListAsync();

                foreach (var prestamo in prestamos)
                {
                    ConsultaPrestamoModel consultaPrestamoModel = new ConsultaPrestamoModel();
                    consultaPrestamoModel.DatosPerfil = ConsultarPerfil(prestamo.IdPerfil);
                    consultaPrestamoModel.Libros = ConsultarLibrosPrestamo(prestamo.IdPerfil);
                    consultaPrestamoModel.FechaPrestamo = prestamo.FechaPrestamo;
                    modeloPrestamos.Add(consultaPrestamoModel);
                }

                return modeloPrestamos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar préstamos: {mensaje}", ex.Message);
                throw new Exception("Error al consultar préstamos");
            }
        }

        private UpdateProfileModel ConsultarPerfil(int id)
        {
            try
            {
                _logger.LogDebug("Consultando perfil con id {id}", id);
                var perfil = _biblioteca.Perfil.FirstOrDefault(p => p.Id == id);
                if (perfil == null)
                {
                    _logger.LogError("Perfil no encontrado");
                    throw new Exception("Perfil no encontrado");
                }
                UpdateProfileModel modeloPerfil = new UpdateProfileModel();
                modeloPerfil.Nombre = perfil.Nombre;
                modeloPerfil.Apellido = perfil.Apellido;
                modeloPerfil.Telefono = perfil.Telefono;
                modeloPerfil.Direccion = perfil.Direccion;
                modeloPerfil.Correo = perfil.Correo;

                _logger.LogInformation("Perfil encontrado con id: {id}", perfil.Id);

                return modeloPerfil;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar perfil {mensaje}", ex.Message);
                throw new Exception("Error al consultar perfil");
            }
        }

    }
}
