using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewVersion_EP.Models;

namespace NewVersion_EP.Controllers
{
    public class UsuarioController : Controller
    {

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Editar()
        {
            return View();
        }

        public ActionResult Opcoes()
        {
            return View();
        }

        public ActionResult ServicosComprados(string userGUID)
        {
            return View();
        }

        public ActionResult MeusServicos(string userGUID)
        {
            return View();
        }

        public ActionResult MeusServicosVendidos(int id)
        {
            return View();
        }

        public ActionResult Inbox(string userGUID)
        {
            return View();
        }

        public ActionResult ResponderPergunta()
        {
            return View();
        }

        public ActionResult ExcluirPergunta()
        {
            return View();
        }
    }
}