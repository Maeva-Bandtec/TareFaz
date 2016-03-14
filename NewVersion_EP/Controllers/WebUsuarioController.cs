using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NewVersion_EP.Models;
using System;
using System.Security.Cryptography;
using System.Web.Security;
using System.Collections.Generic;
using System.Net.Mail;
using System.Data.Entity.Validation;
using NewVersion_EP.Models.DTO;

namespace NewVersion_EP.Controllers
{
    public class WebUsuarioController : ApiController
    {
        private MaevaDBContext db = new MaevaDBContext();
        MailAddress contaEmail = new MailAddress("maeva.inc@gmail.com");
        SmtpClient client = new SmtpClient();

        #region "Actions"
        // GET: api/WebUsuario
        public IQueryable<TB_Usuario> GetTB_Usuario()
        {
            return db.TB_Usuario;
        }

        // GET: api/WebUsuario/5
        [ResponseType(typeof(TB_Usuario))]
        public IHttpActionResult GetTB_Usuario(int id)
        {
            TB_Usuario tB_Usuario = db.TB_Usuario.Find(id);
            if (tB_Usuario == null)
            {
                return NotFound();
            }

            return Ok(tB_Usuario);
        }

        // PUT: api/WebUsuario/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTB_Usuario(int id, TB_Usuario tB_Usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tB_Usuario.Id)
            {
                return BadRequest();
            }

            db.Entry(tB_Usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TB_UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WebUsuario
        [ResponseType(typeof(TB_Usuario))]
        [HttpPost]
        public IHttpActionResult PostTB_Usuario(TB_Usuario usuario)
        {
            try
            {
                var senhaTemp = usuario.Senha;
                usuario.Senha = GerarHash(usuario.Senha);
                usuario.ContaErro = 0;
                usuario.isLocked = false;
                usuario.UserGuid = Guid.NewGuid();
                usuario.DtCriacao = DateTime.Now;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.TB_Usuario.Add(usuario);
                db.SaveChanges();

                #region "Email Cadastrado"
                string template = string.Format("Olá {0},<br><br>Bem vindo ao TareFaz!<br> Você está preparado " +
                    "para navegar pelos mais divertidos serviços da internet?<Br>Então acesse com os seus " +
                    "dados abaixo<br><br><b>Usuário:</b> {1}<br><b>Senha:</b> {2}<br><br>Esperamos por você!" +
                    "<br><br>Atenciosamente,<br>Equipe Maeva", usuario.NomeCompleto, usuario.Email, senhaTemp);

                EnviarEmail(usuario.Email, "Bem vindo ao TareFaz!", template);
                #endregion
            }

            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/WebUsuario/5
        [ResponseType(typeof(TB_Usuario))]
        public IHttpActionResult DeleteTB_Usuario(int id)
        {
            TB_Usuario tB_Usuario = db.TB_Usuario.Find(id);
            if (tB_Usuario == null)
            {
                return NotFound();
            }

            db.TB_Usuario.Remove(tB_Usuario);
            db.SaveChanges();

            return Ok(tB_Usuario);
        }

        [HttpPost]
        public bool PostEmail([FromBody]string email)
        {
            return db.TB_Usuario.Where(u => u.Email == email).Count() > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TB_UsuarioExists(int id)
        {
            return db.TB_Usuario.Count(e => e.Id == id) > 0;
        }

        public string ValidaLogin(TB_Usuario usuario)
        {

            string senhaDigitadaUsuario = usuario.Senha;

            usuario = db.TB_Usuario.Where(x => x.Email == usuario.Email).FirstOrDefault();

            if (usuario == null)
                //Email não cadastrado
                return "NE";

            else if (usuario.isLocked == true)
                //Conta Bloqueada
                return "BQ";

            else if (!ValidarSenha(senhaDigitadaUsuario, usuario.Senha))
            {
                usuario.ContaErro++;

                if (usuario.ContaErro >= 5)
                {
                    usuario.isLocked = true;
                    usuario.ContaErro = 0;
                }
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                //Senha Invalida
                return "SI";
            }

            else if (!usuario.isAtivo)
                //Conta inativa
                return "IN";

            else
            {
                usuario.ContaErro = 0;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return usuario.UserGuid.ToString() + "," + usuario.SexoId;
            }

        }

        public bool EsqueciSenha(TB_Usuario usuario)
        {
            string novaSenha = GerarSenhaAleatoria();

            string hash = GerarHash(novaSenha);

            usuario = db.TB_Usuario.Where(x => x.Email == usuario.Email).FirstOrDefault();

            try
            {
                usuario.Senha = hash;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var grava = "Property: {0} " + validationError.PropertyName + "Error: {1} " + validationError.ErrorMessage;
                    }
                }
            }


            string corpo = "Olá, " + usuario.NomeCompleto + ",<br>Não acredito que você esqueceu sua senha !!! " +
            "Aposto que se fosse a senha do seu cartão de crédito você não teria esquecido. tsc, tsc, tsc. <br>" +
            "Mas então meu jovem, segue o seu usuário e a nova senha. <br><br><strong>Usuário:</strong> " +
            usuario.Email + "<br><strong>Senha:</strong> " + novaSenha + "<br><br>Atenciosamente,<br>Time TareFaz";

            string assunto = "Acesso - Nova Senha Tarefaz";

            try
            {
                EnviarEmail(usuario.Email, assunto, corpo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public string GerarSenhaAleatoria()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
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

        public IHttpActionResult GetTB_Sexo()
        {
            return Ok(db.TB_Sexo.Select(s => new Sexo
            {
                Id = s.Id,
                Descricao = s.Descricao
            })
            .ToList());
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public IHttpActionResult MensagensRecebidas(string userGUID)
        {
            var consulta = (from PV in db.TB_PerguntasVendedor
                            join S in db.TB_Servicos on PV.ServicoId equals S.Id
                            join U in db.TB_Usuario on S.UsuarioId equals U.Id
                            where U.UserGuid.ToString() == userGUID && PV.DtResposta == null && PV.isAtivo == true
                            select new
                            {
                                DataPergunta = PV.DtPergunta,
                                Pergunta = PV.Pergunta,
                                Servico = S.Titulo,
                                Comprador = db.TB_Usuario.Where(x => x.Id == PV.CompradorId).Select(x => x.NomeCompleto),
                                IdPergunta = PV.Id
                            }).ToList();

            return Ok(consulta);
        }

        public bool ResponderPergunta(int idPergunta, string Desc)
        {

            try
            {
                var pergunta = db.TB_PerguntasVendedor.Where(x => x.Id == idPergunta).FirstOrDefault();

                pergunta.Resposta = Desc;
                pergunta.isAtivo = true;
                pergunta.DtResposta = DateTime.Now;
                db.Entry(pergunta).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public bool ExcluirPergunta(int idPergunta)
        {
            try
            {
                var pergunta = db.TB_PerguntasVendedor.Where(x => x.Id == idPergunta).FirstOrDefault();

                pergunta.isAtivo = false;
                db.Entry(pergunta).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GuidRetornaIdUsuario(string GUID)
        {
            return db.TB_Usuario.FirstOrDefault(x => x.UserGuid.ToString() == GUID).Id;

        }

        public string IdUsuarioRetornaGUID(int idUsuario)
        {
            return db.TB_Usuario.FirstOrDefault(x => x.Id == idUsuario).UserGuid.ToString();

        }
        #endregion

        #region "Seguranca"
        public const int saltBytes = 24;
        public const int hashBytes = 24;
        public const int iterPBKDF2 = 1000;

        public const int iterIndex = 0;
        public const int saltIndex = 1;
        public const int pbkdf2Index = 2;

        public static string GerarHash(string senha)
        {
            RNGCryptoServiceProvider rngServCripto = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltBytes];
            rngServCripto.GetBytes(salt);

            byte[] hash = PBKDF2(senha, salt, iterPBKDF2, hashBytes);
            return iterPBKDF2 + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        public static bool ValidarSenha(string senha, string hashCorreto)
        {
            char[] delimitador = { ':' };
            string[] split = hashCorreto.Split(delimitador);
            int iteracoes = Int32.Parse(split[iterIndex]);
            byte[] salt = Convert.FromBase64String(split[saltIndex]);
            byte[] hash = Convert.FromBase64String(split[pbkdf2Index]);

            byte[] testeHash = PBKDF2(senha, salt, iteracoes, hash.Length);
            return SlowEquals(hash, testeHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diferenca = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diferenca |= (uint)(a[i] ^ b[i]);
            return diferenca == 0;
        }

        private static byte[] PBKDF2(string senha, byte[] salt, int iteracoes, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(senha, salt);
            pbkdf2.IterationCount = iteracoes;
            return pbkdf2.GetBytes(outputBytes);
        }
        #endregion
    }
}