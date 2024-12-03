using biblioteca_backend.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections;
using System.IO.Compression;

namespace biblioteca_backend.Clases
{
    public class clsArchivo
    {
        private ILogger<clsArchivo> _logger;
        private readonly DBBibliotecaContext _biblioteca;
        public clsArchivo(DBBibliotecaContext biblioteca, ILogger<clsArchivo> logger)
        {
            _biblioteca = biblioteca;
            _logger = logger;
        }

        public async Task<List<Archivo>> ConsultarTodos()
        {
            try
            {
                _logger.LogDebug("Consultando archivos sin eliminar...");
                List<Archivo> listaArchivos = await _biblioteca.Archivo.Where(A => A.FechaEliminacion == null).OrderByDescending(A => A.FechaCarga).ToListAsync();
                _logger.LogInformation("Consulta de archivos exitosa");
                return listaArchivos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar archivos: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> ConsultarTodosZip()
        {
            try
            {
                _logger.LogDebug("Consultando archivos en una lista...");
                var lista = await _biblioteca.Archivo.Where(A => A.FechaEliminacion==null).ToListAsync();
                var archivosComprimidos = await ComprimirArchivos(lista, "Archivos\\archivoComprimido.zip");

                _logger.LogInformation("Consulta de archivos comprimidos exitosa");
                return archivosComprimidos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar archivos comprimidos: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<byte[]> ComprimirArchivos(List<Archivo> archivos, string _ruta)
        {
            try
            {
                _logger.LogDebug("Comprimiendo archivos en un archivo .zip...");
                string ruta = _ruta;
                using (var zipToCreate = new FileStream(ruta, FileMode.Create))
                using (var archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                {

                    foreach (var archivo in archivos)
                    {
                        var filePath = archivo.RutaDocumento;
                        if (File.Exists(filePath))
                        {
                            var zipEntry = archive.CreateEntry(archivo.RutaDocumento);
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            using (var entryStream = zipEntry.Open())
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                }

                var archivoComprimido = await File.ReadAllBytesAsync(ruta);
                File.Delete(ruta);
                _logger.LogInformation("Archivos comprimidos exitosamente");
                return archivoComprimido;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al comprimir archivos: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CargarArchivo(IFormFile archivo, int idPerfil)
        {
            try
            {
                _logger.LogDebug("Cargando archivo nuevo al sistema...");
                if (!EsDocumento(archivo.FileName))
                {
                    _logger.LogWarning("El tipo de archivo no es válido");
                    return "El tipo de archivo no es válido";
                }
                string ruta = "Archivos\\" + archivo.FileName;
                if (await _biblioteca.Archivo.FirstOrDefaultAsync(A => A.RutaDocumento==ruta && A.FechaEliminacion==null)!=null)
                {
                    _logger.LogWarning("Ya existe un archivo con el mismo nombre");
                    return "Ya existe este archivo en el sistema";
                }
                _logger.LogDebug("Creando archivo en la ruta: ${ruta}...", ruta);
                using (var stream = File.Create(ruta))
                {
                    archivo.CopyTo(stream);
                }
                Archivo _archivo = new Archivo();
                _archivo.Nombre = archivo.FileName;
                _archivo.FechaCarga = DateTime.Now;
                _archivo.RutaDocumento = ruta;

                await _biblioteca.Archivo.AddAsync(_archivo);
                await _biblioteca.SaveChangesAsync();

                await RegistrarAccion(_archivo.Id, idPerfil, 1);
                _logger.LogInformation("Archivo cargado exitosamente");
                return "Archivo guardado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al cargar el archivo: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private static bool EsDocumento(string ruta)
        {
            bool esDocumento = false;
            var arreglo = ruta.Split('.');
            string extension = arreglo[arreglo.Length-1].ToLower();
            if (extension == "pdf" ||
                extension == "doc" ||
                extension == "docx" ||
                extension == "xls" ||
                extension == "xlsx" ||
                extension == "txt" ||
                extension == "csv"
                )
            {
                esDocumento = true;
            }
            return esDocumento;
        }

        private async Task RegistrarAccion(int idArchivo, int idPerfil, int idAccion)
        {
            try
            {
                RegistroArchivoPerfil registroArchivo = new RegistroArchivoPerfil();
                registroArchivo.IdAccion = idAccion;
                registroArchivo.IdArchivo = idArchivo;
                registroArchivo.IdPerfil = idPerfil;
                registroArchivo.Fecha = DateTime.Now;
                await _biblioteca.RegistroArchivoPerfil.AddAsync(registroArchivo);
                await _biblioteca.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> EliminarArchivo(int idArchivo, int idPerfil)
        {
            try
            {
                _logger.LogDebug("Eliminando archivo con id: ${id}...", idArchivo);
                var archivo = await _biblioteca.Archivo.FirstOrDefaultAsync(a => a.Id == idArchivo);
                if (archivo == null || archivo.FechaEliminacion!=null)
                {
                    _logger.LogError("El archivo no fue encontrado");
                    throw new Exception("Archivo no encontrado");
                }
                archivo.FechaEliminacion = DateTime.Now;
                _biblioteca.Archivo.Update(archivo);
                await _biblioteca.SaveChangesAsync();

                File.Delete(archivo.RutaDocumento);

                await RegistrarAccion(archivo.Id, idPerfil, 3);
                _logger.LogInformation("Archivo eliminado exitosamente");
                return "Archivo eliminado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al eliminar archivo: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> DescargarArchivo(int idArchivo, int idPerfil)
        {
            try
            {
                _logger.LogDebug("Descargando archivo...");
                var archivo = await _biblioteca.Archivo.FirstOrDefaultAsync(A => A.Id == idArchivo);
                if (archivo == null)
                {
                    _logger.LogWarning("Archivo no encontrado");
                    throw new Exception("Archivo no encontrado");
                }
                await RegistrarAccion(archivo.Id, idPerfil, 2);

                archivo.NumeroDescargas  ++;
                _biblioteca.Archivo.Update(archivo);
                await _biblioteca.SaveChangesAsync();

                byte[] archivoBytes = File.ReadAllBytes(archivo.RutaDocumento);

                _logger.LogInformation("Archivo descargado exitosamente");
                return archivoBytes;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al descargar archivo: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable> ConsultarHistorial()
        {
            try
            {
                _logger.LogDebug("Consultando historial de registros de archivos...");
                var consulta = await (from A in _biblioteca.Archivo
                               join RA in _biblioteca.RegistroArchivoPerfil
                               on A.Id equals RA.IdArchivo
                               join P in _biblioteca.Perfil
                               on RA.IdPerfil equals P.Id
                               join AC in _biblioteca.Accion
                               on RA.IdAccion equals AC.Id
                               orderby RA.Fecha descending
                               select new
                               {
                                   id = RA.Id,
                                   nombreArchivo = A.Nombre,
                                   idAccion = AC.Id,
                                   accion = AC.Nombre,
                                   nombreUsuario = P.Nombre+' '+P.Apellido,
                                   fechaAccion = RA.Fecha
                               }).ToListAsync();
                _logger.LogInformation("Consulta de historial exitosa");
                return consulta;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar historial de archivos: ${mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
