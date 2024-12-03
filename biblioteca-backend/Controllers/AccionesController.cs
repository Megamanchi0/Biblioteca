using biblioteca_backend.Clases;
using biblioteca_backend.Models;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionesController : ControllerBase
    {
        private clsAccion accion;
        public AccionesController(DBBibliotecaContext biblioteca)
        {
            accion = new clsAccion(biblioteca);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await accion.ConsultarAcciones());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
