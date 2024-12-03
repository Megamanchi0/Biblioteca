using biblioteca_backend.Clases;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenerosController : ControllerBase
    {
        clsGenero _genero;
        public GenerosController(DBBibliotecaContext biblioteca)
        {
            _genero = new clsGenero(biblioteca);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _genero.ConsultarTodos();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
