using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;

namespace Octopus.Controllers
{
    public class HomeController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        // GET: /Home/

        public ActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        public ActionResult Home()
        {
           return View( db.VW_Statistics.ToList());
        }

    }
}
