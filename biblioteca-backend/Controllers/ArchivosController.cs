using biblioteca_backend.Clases;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private clsArchivo _archivo;
        public ArchivosController(DBBibliotecaContext biblioteca, ILogger<clsArchivo> logger) 
        { 
            _archivo = new clsArchivo(biblioteca, logger);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CargarDocumento(IFormFile archivo, int idPerfil)
        {
            try
            {
                var respuesta = await _archivo.CargarArchivo(archivo, idPerfil);

                return Ok(new {message = respuesta});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarDocumento(int idArchivo, int idPerfil)
        {
            try
            {
                var respuesta = await _archivo.EliminarArchivo(idArchivo, idPerfil);
                return Ok(new {message = respuesta});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DescargarDocumento(int idArchivo, int idPerfil)
        {
            try
            {
                var respuesta = await _archivo.DescargarArchivo(idArchivo, idPerfil);
                return File(respuesta, "application/octet-stream");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConsultarDocumentosZip")]
        [Authorize]
        public async Task<IActionResult> ConsultarTodosZip()
        {
            try
            {
                var resultado = await _archivo.ConsultarTodosZip();
                return File(resultado, "application/zip");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConsultarDocumentos")]
        [Authorize]
        public async Task<IActionResult> ConsultarTodos()
        {
            try
            {
                var resultado = await _archivo.ConsultarTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConsultarHistorial")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ConsultarHistorial()
        {
            try
            {
                var resultado = await _archivo.ConsultarHistorial();
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
