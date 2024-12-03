using biblioteca_backend.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace biblioteca_backend.Clases
{
    public class ServicioCorreo
    {
        private IConfiguration configuration;
        private ILogger logger;
        public ServicioCorreo(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<Response> EnviarCorreo(string to, string subject, string html, string plainTextContent)
        {
            try
            {
                logger.LogDebug("Enviando email al correo: {email}...", to);
                var apiKey = Environment.GetEnvironmentVariable("API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("biblioteca.medellin1923@gmail.com", "Biblioteca");
                var asunto = subject;
                var a = new EmailAddress(to, "Receptor");
                var htmlContent = html;
                var msg = MailHelper.CreateSingleEmail(from, a, asunto, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("Email enviado a {email} exitosamente",to);
                }
                else
                {
                    logger.LogWarning("No fue posible enviar el email al correo {email}", to);
                }
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError("Error al enviar correo: {mensaje}", ex.Message);
                throw new Exception("Error al enviar correo");
            }
        }
    }
}
