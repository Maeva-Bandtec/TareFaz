using NewVersion_EP.Models;
using NewVersion_EP.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace NewVersion_EP.Controllers
{
    public class WebAdminController : ApiController
    {
        private MaevaDBContext db = new MaevaDBContext();
        MailAddress contaEmail = new MailAddress("maeva.inc@gmail.com");
        SmtpClient client = new SmtpClient();

        public IHttpActionResult PostTB_FaleConosco(TB_FaleConosco mensagem)
        {
            mensagem.DtCriacao = DateTime.Now;
            db.TB_FaleConosco.Add(mensagem);
            db.SaveChanges();

            try
            {
                string msgFaleConosco = string.Format("Olá!<br>Sua mensagem foi enviada e será analisada pela equipe do Tarefaz!<br> Agradecemos o contato e esperamos que se divirta em nosso site aproveitando os serviços disponíveis. <br><br>Atenciosamente,<br>Equipe Maeva");

                EnviarEmail(mensagem.EmailRemetente, "Sua mensagem foi enviada à equipe do Tarefaz", msgFaleConosco);
            }
            catch (Exception)
            {
                throw;
            }
            return StatusCode(HttpStatusCode.OK);

        }

        public void EnviarEmail(string email, string assunto, string corpo)
        {
            try
            {

                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                NetworkCredential basicauthenticationinfo = new NetworkCredential("maeva.inc@gmail.com", "maeva@123456");
                client.Credentials = basicauthenticationinfo;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage msg = new MailMessage();
                msg.Subject = assunto;

                msg.Body = corpo;

                msg.From = contaEmail;
                msg.To.Add(email);
                msg.IsBodyHtml = true;

                client.Send(msg);


            }
            catch (Exception)
            {

            }
        }

        public IHttpActionResult GetTB_FaleConoscoAssunto()
        {
            return Ok(db.TB_FaleConoscoAssunto
                .Select(c => new FaleConoscoAssunto
                {
                    Id = c.Id,
                    Assunto = c.Assunto
                })
                .ToList());
        }
    }
}
