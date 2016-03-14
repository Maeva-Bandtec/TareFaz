using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewVersion_EP.Controllers
{
    public class ServicoController : Controller
    {
        // GET: Servico
        public ActionResult Pagina(int id = 1)
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

        public ActionResult Detalhes()
        {
            return View();
        }             
    }
}