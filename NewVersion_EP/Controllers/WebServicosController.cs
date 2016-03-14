using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NewVersion_EP.Models;
using NewVersion_EP.Models.DTO;
using System.IO;
using Newtonsoft.Json.Linq;

namespace NewVersion_EP.Controllers
{
    public class WebServicosController : ApiController
    {
        private MaevaDBContext db = new MaevaDBContext();

        #region "Actions"

        [HttpPost]
        public IHttpActionResult Get_MeusServicos([FromBody]string userGUID)
        {
            int idUsuario = GuidRetornaIdUsuario(userGUID);

            return Ok(db.TB_Servicos
                .Where(s => s.UsuarioId == idUsuario)
                .Select(s => new MeusServicos
                {
                    Id = s.Id,

                    Titulo = s.Titulo,

                    QtdVendidos = db.TB_ServicosVendidos.Where(sv => sv.ServicoId == s.Id).Count(),

                    QtdEntregues = db.TB_ServicosVendidos.Where(sv => sv.ServicoId == s.Id &&
                        sv.DtEntrega != null && sv.CaminhoArquivo != null).Count(),

                    QtdPendentes = db.TB_ServicosVendidos.Where(sv => sv.ServicoId == s.Id).Count() - db.TB_ServicosVendidos.Where(sv => sv.ServicoId == s.Id &&
                    sv.DtEntrega != null && sv.CaminhoArquivo != null).Count(),

                    IsAtivo = s.isAtivo
                }).ToList());
        }

        // GET: api/WebServicos
        public IQueryable<TB_Servicos> GetTB_Servicos()
        {
            return db.TB_Servicos;
        }

        // GET: api/WebServicos/5
        [ResponseType(typeof(TB_Servicos))]
        public IHttpActionResult GetTB_Servicos(int id)
        {
            return Ok(db.TB_Servicos
                .Where(s => s.Id == id)
                .Select(s => new Servico
            {
                Id = s.Id,
                UsuarioId = s.UsuarioId,
                CategoriaId = s.CategoriaId,
                Titulo = s.Titulo,
                Descricao = s.Descricao,
                Foto = s.Foto,
                TempoEntrega = s.TempoEntrega,
                Tag = s.Tag,
                Video = s.Video,
                Instrucoes = s.Instrucoes,
                TemArquivo = s.TemArquivo,
                DtAtivacao = s.DtAtivacao,
                isAtivo = s.isAtivo,

                Categoria = db.TB_Categoria
                .Where(c => c.Id == s.CategoriaId)
                .Select(c => new Categoria
                {
                    Id = c.Id,
                    NomeCategoria = c.NomeCategoria
                }).FirstOrDefault(),

                PerguntasVendedor = db.TB_PerguntasVendedor
                .Where(pv => pv.ServicoId == s.Id && pv.isAtivo == true)
                .Select(pv => new PerguntasVendedor
                {
                    Id = pv.Id,
                    ServicoId = pv.ServicoId,
                    CompradorId = pv.CompradorId,
                    Pergunta = pv.Pergunta,
                    DtPergunta = pv.DtPergunta,
                    Resposta = pv.Resposta,
                    DtResposta = pv.DtResposta,
                    isAtivo = pv.isAtivo,

                    Usuario = db.TB_Usuario
                    .Where(u => u.Id == pv.CompradorId)
                    .Select(u => new Usuario
                    {
                        Id = u.Id,
                        NomeCompleto = u.NomeCompleto
                    }).FirstOrDefault()

                }).ToList(),

                Usuario = db.TB_Usuario
                .Where(u => u.Id == s.UsuarioId)
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    NomeCompleto = u.NomeCompleto
                }).FirstOrDefault()
            }).FirstOrDefault());
        }

        // PUT: api/WebServicos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTB_Servicos(int id, TB_Servicos tB_Servicos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tB_Servicos.Id)
            {
                return BadRequest();
            }

            //db.Entry(tB_Servicos).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TB_ServicosExists(id))
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


        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult PostTB_Servicos(TB_Servicos tB_Servicos)
        {
            try
            {
                tB_Servicos.DtAtivacao = DateTime.Now;

                var teste = db.TB_Servicos.Find(db.TB_Servicos.Count());

                int[] ids = new int[2];

                db.TB_Servicos.Add(tB_Servicos);
                db.SaveChanges();

                ids[0] = tB_Servicos.UsuarioId;
                ids[1] = db.TB_Servicos.OrderByDescending(s => s.Id).FirstOrDefault().Id;

                tB_Servicos = db.TB_Servicos.Find(ids[1]);
                tB_Servicos.Foto = "Usuario_" + ids[0] + "/Servico_" + ids[1] + "/Galeria/01.jpg";

                db.SaveChanges();

                return Ok(ids);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/WebServicos/5
        [ResponseType(typeof(TB_Servicos))]
        public IHttpActionResult DeleteTB_Servicos(int id)
        {
            TB_Servicos tB_Servicos = db.TB_Servicos.Find(id);
            if (tB_Servicos == null)
            {
                return NotFound();
            }

            db.TB_Servicos.Remove(tB_Servicos);
            db.SaveChanges();

            return Ok(tB_Servicos);
        }

        public IHttpActionResult GetTB_Categoria()
        {
            return Ok(db.TB_Categoria
                .Select(c => new Categoria
                {
                    Id = c.Id,
                    NomeCategoria = c.NomeCategoria
                })
                .ToList());
        }

        public IHttpActionResult GetTB_ServicosLista(int id = 1, int categoria = 0)
        {
            if (categoria != 0)
                return Ok(db.TB_Servicos
                .Where(s => s.isAtivo == true && s.CategoriaId == categoria)
                .OrderBy(s => s.Id)
                .Skip(6 * (id - 1))
                .Take(6)
                .Select(s => new Servico
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    CategoriaId = s.CategoriaId,
                    Titulo = s.Titulo,
                    Descricao = s.Descricao,
                    Foto = s.Foto,
                    TempoEntrega = s.TempoEntrega,
                    Tag = s.Tag,
                    Video = s.Video,
                    Instrucoes = s.Instrucoes,
                    TemArquivo = s.TemArquivo,
                    DtAtivacao = s.DtAtivacao,
                    isAtivo = s.isAtivo
                }).ToList());

            return Ok(db.TB_Servicos
                .Where(s => s.isAtivo == true)
                .OrderBy(s => s.Id)
                .Skip(6 * (id - 1))
                .Take(6)
                .Select(s => new Servico
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    CategoriaId = s.CategoriaId,
                    Titulo = s.Titulo,
                    Descricao = s.Descricao,
                    Foto = s.Foto,
                    TempoEntrega = s.TempoEntrega,
                    Tag = s.Tag,
                    Video = s.Video,
                    Instrucoes = s.Instrucoes,
                    TemArquivo = s.TemArquivo,
                    DtAtivacao = s.DtAtivacao,
                    isAtivo = s.isAtivo,
                }).ToList());
        }

        [ResponseType(typeof(TB_PerguntasVendedor))]
        public IHttpActionResult PostTB_PerguntasVendedor(TB_PerguntasVendedor pergunta)
        {
            try
            {
                pergunta.DtPergunta = DateTime.Now;

                db.TB_PerguntasVendedor.Add(pergunta);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.OK);
        }

        public int GetTB_ServicosLength(int id = 0)
        {
            if (id == 0)
                return db.TB_Servicos
                    .Where(s => s.isAtivo == true)
                    .Count();
            else
                return db.TB_Servicos
                    .Where(s => s.isAtivo == true && s.CategoriaId == id)
                    .Count();
        }

        public IHttpActionResult PostTB_ServicosVendidos(TB_ServicosVendidos compra)
        {
            compra.DtPedido = DateTime.Now;

            db.TB_ServicosVendidos.Add(compra);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.OK);
        }

        [HttpPost]
        public IHttpActionResult GetTB_ServicosVendidos(JObject objDados)
        {
            int idUsuario = GuidRetornaIdUsuario(objDados["userGUID"].ToString());
            int id = objDados["id"].ToObject<int>();

            var consulta = db.TB_ServicosVendidos
                .Where(sv => sv.TB_Servicos.UsuarioId == idUsuario && sv.TB_Servicos.Id == id && sv.TB_Servicos.isAtivo == true)
                .Select(sv => new ServicoVendido
                {
                    Id = sv.Id,
                    ServicoId = sv.ServicoId,
                    CompradorId = sv.CompradorId,
                    DtPedido = sv.DtPedido,
                    DtEntrega = sv.DtEntrega,
                    CaminhoArquivo = sv.CaminhoArquivo,
                    isAtivo = sv.isAtivo,

                    Servico = db.TB_Servicos
                    .Where(s => s.Id == sv.ServicoId)
                    .Select(s => new Servico
                    {
                        Id = s.Id,
                        Titulo = s.Titulo,
                        UsuarioId = s.UsuarioId
                    }).FirstOrDefault(),

                    Usuario = db.TB_Usuario
                    .Where(u => u.Id == sv.CompradorId)
                    .Select(u => new Usuario
                    {
                        Id = u.Id,
                        NomeCompleto = u.NomeCompleto
                    }).FirstOrDefault()

                }).ToList();

            return Ok(consulta);
        }

        //public IHttpActionResult GetTB_ServicosByProc(int categoria, int pElem, int uElem)
        //{
        //    string connString = System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;

        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString);
        //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("JP_Pagination", conn);
        //    System.Data.SqlClient.SqlDataReader rdr;

        //    cmd.Parameters.Add("@Categoria", SqlDbType.Int).Value = categoria;
        //    cmd.Parameters.Add("@primeiroElemento", SqlDbType.Int).Value = pElem;
        //    cmd.Parameters.Add("@ultimoElemento", SqlDbType.Int).Value = uElem;

        //    conn.Open();
        //    rdr = cmd.ExecuteReader();

        //    IQueryable<TB_Servicos> listaServicos;

        //    while (rdr.Read())
        //    {

        //    }

        //    conn.Close();
        //    conn.Dispose();

        //    return Ok();
        //}

        public IHttpActionResult Post_Foto(int id, int idServico)
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var arquivo = System.Web.HttpContext.Current.Request.Files["arquivo"];

                if (arquivo != null)
                {
                    string caminho = System.IO.Path.Combine(System.Web.HttpContext.Current.Server
                        .MapPath("~/Content/Uploads/Usuario") + "_" + id + "\\Servico_" + idServico + "\\Galeria\\");

                    Directory.CreateDirectory(caminho);

                    arquivo.SaveAs(caminho + "01" + System.IO.Path.GetExtension(arquivo.FileName));
                    return StatusCode(HttpStatusCode.OK);
                }
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        public IHttpActionResult Post_Arquivo(int idVendedor, int idComprador, int idServico)
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var arquivo = System.Web.HttpContext.Current.Request.Files["arquivo"];

                if (arquivo != null)
                {
                    string caminho = System.IO.Path.Combine(System.Web.HttpContext.Current.Server
                        .MapPath("~/Content/Uploads/Usuario") + "_" + idVendedor + "\\Servico_" + idServico + "\\Entregas\\Comprador_" + idComprador);

                    Directory.CreateDirectory(caminho);

                    arquivo.SaveAs(caminho + "/01" + System.IO.Path.GetExtension(arquivo.FileName));

                    var servicoVendido = db.TB_ServicosVendidos
                        .Where(sv => sv.CompradorId == idComprador && sv.TB_Servicos.UsuarioId == idVendedor && sv.TB_Servicos.Id == idServico)
                        .FirstOrDefault();

                    servicoVendido.CaminhoArquivo = "Usuario_" + idVendedor + "/Servico_" + idServico + "/Entregas/Comprador_" + idComprador + "/01" + System.IO.Path.GetExtension(arquivo.FileName);
                    servicoVendido.DtEntrega = DateTime.Now;
                    db.SaveChanges();

                    #region EmailPedreiro(AlterarASAP)
                    System.Net.Mail.MailAddress contaEmail = new System.Net.Mail.MailAddress("maeva.inc@gmail.com");
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    NetworkCredential basicauthenticationinfo = new NetworkCredential("maeva.inc@gmail.com", "maeva@123456");
                    client.Credentials = basicauthenticationinfo;

                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                    msg.Subject = "Tarefaz - Sua compra foi entregue!";

                    msg.Body = @"Olá!<br><br>
                               Que tal checar o seu novo produto? <br>Acesse http://tarefaz.azurewebsites.net/Content/Uploads/"+ servicoVendido.CaminhoArquivo + @" e confira!<br><br>
                               Atenciosamente,<br>
                               Equipe Maeva";

                    msg.From = contaEmail;
                    msg.To.Add(db.TB_Usuario.Where(u => u.Id == idComprador).Select(u => u.Email).FirstOrDefault());
                    msg.IsBodyHtml = true;

                    client.Send(msg);

                    #endregion

                    return StatusCode(HttpStatusCode.OK);
                }
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public IHttpActionResult AtualizaStatusServicos([FromBody]string[] vetorIds)
        {
            try
            {
                for (int i = 0; i < vetorIds.Length; i++)
                {
                    var servico = db.TB_Servicos.Find(Int32.Parse(vetorIds[i]));

                    servico.isAtivo = servico.isAtivo == true ? false : true;
                }
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            
        }

        [HttpPost]
        public IHttpActionResult PutTB_ServicosVendidos(string jsonIDs)
        {
            return StatusCode(HttpStatusCode.OK);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TB_ServicosExists(int id)
        {
            return db.TB_Servicos.Count(e => e.Id == id) > 0;
        }

        [HttpPost]
        public int GuidRetornaIdUsuario([FromBody]string userGUID)
        {
            return db.TB_Usuario.FirstOrDefault(x => x.UserGuid.ToString() == userGUID).Id;
        }

        [AcceptVerbs("GET", "POST")]
        public string IdUsuarioRetornaGUID([FromBody]int idUsuario)
        {
            return db.TB_Usuario.FirstOrDefault(x => x.Id == idUsuario).UserGuid.ToString();

        }
    }
}