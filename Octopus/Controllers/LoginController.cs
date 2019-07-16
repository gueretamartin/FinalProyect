using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using System.Dynamic;
using System.Web.Security;


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
        [ValidateAntiForgeryToken]  
        public ActionResult Login(USUARIOS objUser)   
        {  
            if (ModelState.IsValid)   
            {  
                using(OctopusEntities db = new OctopusEntities())  
                {  
                    var obj = db.USUARIOS.Where(a => a.Usuario.Equals(objUser.Usuario) && a.Contraseña.Equals(objUser.Contraseña)).FirstOrDefault();  
                    if (obj != null)  
                    {  
                        Session["UserName"] = obj.Usuario.ToString();  
                        Session["Rol"] = obj.Rol.ToString();
                        FormsAuthentication.SetAuthCookie(obj.Usuario, true);
                        return RedirectToAction("Home");  
                    }
                    else { ModelState.AddModelError("Usuario", "El usuario y/o la contraseña son incorrectos"); }
                }  
            }  
            return View();  
        }  
  
        public ActionResult Home()  
        {  
            if (Session["UserName"] != null)  
            {
                
                return RedirectToAction("Home", "Home");
            } else  
            {
                ModelState.AddModelError("Usuario", "El usuario y/o la contraseña son incorrectos");
                return View();  
            }  
        }

        
        public ActionResult LogOut()
        {

            Session["UserName"] = null;
            Session["Rol"] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }
          
        }

    }
