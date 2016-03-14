using NewVersion_EP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Security.Cryptography;
using System.Web.Security;
using System.Net.Mail;

namespace NewVersion_EP.Controllers
{
    public class FaleConoscoController : Controller
    {
        // GET: FaleConosco
        public ActionResult Index()
        {
            return View();
        }
    }
}