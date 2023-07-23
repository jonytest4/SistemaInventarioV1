using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Utilidades
{
    /*clase para hacer funcionar el EmailSender si aún no se lo configura en el modelo Register de la página razer Register
    pasarlo como servicio en el program.cs
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }*/

    //Clases y porpiedades si ya se cuenta con datos necesarios para configurar SendGrid
    
    public class EmailSender : IEmailSender
    {
        //propiedad para capturar la clave
        public string SendGridSecret { get; set; }
        //constructor para acceder appsettings
        public EmailSender(IConfiguration _config){
            SendGridSecret = _config.GetValue<string>("Sendgrid:SecretKey"); 
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("jonathantito54@gmail.com");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);//variable mensaje

            return client.SendEmailAsync(msg);
        }
    }
}
