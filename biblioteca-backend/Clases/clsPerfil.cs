
using biblioteca_backend.Models;
using biblioteca_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace biblioteca_backend.Clases
{
    public class clsPerfil
    {
        private IConfiguration configuration;
        private readonly DBBibliotecaContext biblioteca;
        private ServicioCorreo servicioCorreo;
        private readonly ILogger<clsPerfil> _logger;
        public clsPerfil(IConfiguration configuration, DBBibliotecaContext biblioteca, ILogger<clsPerfil> logger)
        {
            this.configuration = configuration;
            this.biblioteca = biblioteca;
            servicioCorreo = new ServicioCorreo(configuration, logger);
            _logger = logger;
        }

        public dynamic IniciarSesion(LogInModel logInModel)
        {
            try
            {
                _logger.LogDebug("Iniciando sesión...");
                string claveCifrada = Cifrar.EncriptarClave(logInModel.Contrasena);
                var _perfil = biblioteca.Perfil.FirstOrDefault(p => p.Correo == logInModel.Correo && p.Contrasena == claveCifrada);
                if (_perfil == null)
                {
                    _logger.LogWarning("Correo o contraseña no encontrados");
                    return "";
                }
                var key = configuration.GetSection("Jwt").GetSection("Key").Value;
                byte[] bytekey = Encoding.UTF8.GetBytes(key!);
                var tokenHandler = new JwtSecurityTokenHandler();
                var rol = biblioteca.Rol.FirstOrDefault(r => r.Id == _perfil.IdRol);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim ("id", _perfil.Id.ToString()),
                    new Claim (ClaimTypes.Email, _perfil.Correo),
                    new Claim (ClaimTypes.Role, rol!.Nombre)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytekey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescription);

                _logger.LogInformation("Inicio de sesión exitoso");

                return new { result = tokenHandler.WriteToken(token) };

            }
            catch (Exception ex)
            {
                _logger.LogError("Error al iniciar sesión: {mensaje}", ex.Message);
                throw new Exception("Error al iniciar sesión");
            }
            
        }

        public async Task<string> Registrarse(Perfil perfil)
        {
            try
            {
                _logger.LogDebug("Iniciando registro de perfil...");
                var _perfil = biblioteca.Perfil.FirstOrDefault(p => p.Correo == perfil.Correo);
                if (_perfil != null)
                {
                    _logger.LogWarning("Correo ya registrado");
                   return "";
                }
                string claveCifrada = Cifrar.EncriptarClave(perfil.Contrasena);
                perfil.Contrasena = claveCifrada;
                perfil.IdRol = 2;
                await biblioteca.Perfil.AddAsync(perfil);
                await biblioteca.SaveChangesAsync();

                string subject = "Confirmación de registro";
                string html = "<strong>¡Registro exitoso!</strong><p>El registro en el portal de la biblioteca ha sido exitoso</p>";
                string textoPlano = "El registro en el portal de la biblioteca ha sido exitoso";
                await servicioCorreo.EnviarCorreo(perfil.Correo, subject, html, textoPlano);
                _logger.LogInformation("Registro de perfil exitoso");
                return "Registro realizado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al realizar registro: {mensaje}", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ActualizarPerfil(UpdateProfileModel perfil)
        {
            try
            {
                _logger.LogDebug("Actualizando perfil...");
                var _perfil = await biblioteca.Perfil.FirstOrDefaultAsync(p => p.Id == perfil.Id);
                if (_perfil == null)
                {
                    _logger.LogWarning("Perfil no encontrado");
                    throw new Exception("Perfil no encontrado");
                }

                _perfil.Nombre = perfil.Nombre;
                _perfil.Apellido = perfil.Apellido;
                _perfil.Direccion = perfil.Direccion;
                _perfil.Telefono = perfil.Telefono;

                biblioteca.Perfil.Update(_perfil);
                biblioteca.SaveChanges();
                _logger.LogInformation("Perfil actualizado con id: {_perfil.Id}", _perfil);
                return "Perfil actualizado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar datos: {mensaje}", ex.Message);
                throw new Exception("Error al actualizar los datos");
            }
        }

        public UpdateProfileModel ConsultarPerfil(int id)
        {
            try
            {
                _logger.LogDebug("Consultando perfil con id {id}", id);
                var perfil = biblioteca.Perfil.FirstOrDefault(p => p.Id == id);
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
