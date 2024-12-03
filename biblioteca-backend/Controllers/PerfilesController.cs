using biblioteca_backend.Clases;
using biblioteca_backend.Models;
using biblioteca_backend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace biblioteca_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        private clsPerfil _perfil;
       public PerfilesController(IConfiguration configuration, DBBibliotecaContext biblioteca, ILogger<clsPerfil> logger)
        {
            _perfil = new clsPerfil(configuration, biblioteca, logger);
        }

        [HttpPost]
        public dynamic Post([FromBody] LogInModel logInModel)
        {
            try
            {
                return _perfil.IniciarSesion(logInModel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(Perfil perfil)
        {
            try
            {
                var respuesta = await _perfil.Registrarse(perfil);
                if (respuesta=="")
                {
                    return Conflict(new {message = "El correo ingresado ya está registrado"});
                }
                return Ok(new {message=respuesta});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateProfileModel perfil)
        {
            try
            {
                var respuesta = await _perfil.ActualizarPerfil(perfil);
                return Ok(new {message = respuesta});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var respuesta = _perfil.ConsultarPerfil(id);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
