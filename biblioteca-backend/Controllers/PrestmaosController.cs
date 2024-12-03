using biblioteca_backend.Clases;
using biblioteca_backend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestmaosController : ControllerBase
    {
        private clsPrestamo prestamo;

        public PrestmaosController(DBBibliotecaContext biblioteca, ILogger<clsPrestamo> logger)
        {
            prestamo = new clsPrestamo(biblioteca, logger);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(PrestamoModel prestamoModel)
        {
            try
            {
                var respuesta = await prestamo.GuardarPrestamo(prestamoModel);
                return Ok(new {result = respuesta });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(int idPerfil)
        {
            try
            {
                var libros = prestamo.ConsultarLibrosPrestamo(idPerfil);
                return Ok(new {result= libros });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> EliminarPrestamo(int idPerfil)
        {
            try
            {
                var respuesta = await prestamo.EliminarPrestamo(idPerfil);
                return Ok(new { message = respuesta });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles ="Administrador")]
        [Route("ConsultarPrestamos")]
        public async Task<IActionResult> ConsultarPrestamos()
        {
            try
            {
                var prestamos = await prestamo.ConsultarPrestamos();
                return Ok(prestamos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
