using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using System.Dynamic;


namespace Octopus.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(USUARIOS objUser)
        {
            if (ModelState.IsValid)
            {
                using (OctopusEntities1 db = new OctopusEntities1())
                {
                    USUARIOS obj = db.USUARIOS.Where(a => a.Usuario.Equals(objUser.Usuario) && a.Contraseña.Equals(objUser.Contraseña)).FirstOrDefault();
                    if (obj != null)
                    {
                        //Session["UserUser"] = obj.Usuario.ToString();
                        //Session["UserName"] = obj.Nombre.ToString();
                        //@ViewBag.Message = "Bienvenido " + obj.Nombre + ' ' + obj.Apellido;

                        //@ViewBag.Usuario = (Octopus.Models.USUARIOS)obj;

                        //Pasar más de un modelo
                        //ViewData["Usuario"] = obj; -- tanto como viewbag, no se pueden usar porque al redireccionar become null

                        //TempData["Usuario"] = (USUARIOS)obj; --También funciona
                        Session["Usuario"] = obj;
                        return RedirectToAction("Home", "Home");

                        //return View();
                    }
                }
            }
            return View(objUser);
        }

    }
}
