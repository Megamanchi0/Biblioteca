using biblioteca_backend.Clases;
using biblioteca_backend.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections;


namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly clsLibro _libro;
        public LibrosController(DBBibliotecaContext biblioteca, ILogger<clsLibro> logger)
        {
            _libro = new clsLibro(biblioteca, logger);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(bool mostrarActivos)
        {
            try
            {
                var consulta = _libro.ConsultarTodos(mostrarActivos);
                return Ok(consulta);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GET: api/<LibrosController>
        //[HttpGet]
        //[Authorize]
        //public IActionResult Get(int pagina, int numeroRegistros, bool mostrarActivos)
        //{
        //    try
        //    {
        //        var consulta = _libro.ConsultarTodos(pagina, numeroRegistros, mostrarActivos);
        //        return Ok(consulta);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

        // POST api/<LibrosController>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post([FromBody] Libro libro)
        {
            try
            {
                var respuesta = await _libro.Guardar(libro);
                return Ok(new {message = respuesta});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT api/<LibrosController>/5
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Put([FromBody] Libro libro)
        {
            try
            {
                var respuesta = await _libro.Actualizar(libro);
                return Ok(new {message = respuesta });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<LibrosController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var respuesta = await _libro.Eliminar(id);
                if (respuesta =="")
                {
                    return NotFound();
                }
                return Ok( new {message = respuesta });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("ContarLibros")]
        //public IActionResult ContarLibros(bool mostrarActivos)
        //{
        //    try
        //    {
        //        var resultado = _libro.ContarLibros(mostrarActivos);
        //        return Ok(resultado);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
