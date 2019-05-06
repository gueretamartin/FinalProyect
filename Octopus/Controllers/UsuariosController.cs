using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using System.Web.SessionState;

namespace Octopus.Controllers
{
    public class UsuariosController : Controller
    {

        private OctopusEntities db = new OctopusEntities();

        public ActionResult Index()
        {
            return View(db.USUARIOS.ToList());
        }

        public ActionResult Details(String user)
        {
            USUARIOS USUARIOS = db.USUARIOS.Find(user);
            if (USUARIOS == null)
            {
                return HttpNotFound();
            }

            return View(USUARIOS);
        }

        public ActionResult Create()
        {
            var empleados = db.EMPLEADOS.Where(c => c.EMP_USUARIO == null);
            var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            ViewBag.Rol = new SelectList(db.USUARIOS_TIPOS, "ROL_ID", "ROL_DESC");
            ViewBag.Estado = new SelectList(states, "ES_ID", "ES_DESCRIPCION");
            ViewBag.Empleados = new SelectList(empleados, "EMP_ID", "EMP_NOMBRE");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(USUARIOS var_usuario)
        {
            int? emp = var_usuario.emp_id;
            EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.EMP_ID == emp);
            if (empleado != null)
            {
                empleado.EMP_USUARIO = var_usuario.Usuario;
                ModelState.Remove("EMPLEADOS");
            }
            try
            {
                if (!String.IsNullOrEmpty(var_usuario.Usuario))
                {
                    var user = from c in db.USUARIOS select c;

                    user = user.Where(c => c.Usuario.Contains(var_usuario.Usuario));

                    if (user.Count() != 0)
                    {
                        ModelState.AddModelError("Usuario", "USUARIO: El Usuario ingresado ya existe.");
                        return this.Create();
                    }

                }
                else
                {
                    ModelState.AddModelError("Usuario", "Usuario: Por favor, ingrese un nombre de Usuario.");
                    return this.Create();
                }


                if (ModelState.IsValid)
                {

                    db.USUARIOS.Add(var_usuario);
                    db.SaveChanges();
                    if (empleado != null)
                    {
                        db.Entry(var_usuario).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("List");
                }

                return RedirectToAction("List");

            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult List(string searchUser)
        {

            try
            {
                var usuarios = from c in db.USUARIOS where (c.Estado == 1) select c;

                if (!String.IsNullOrEmpty(searchUser))
                {
                    usuarios = usuarios.Where(c => c.Usuario.Contains(searchUser));
                }

                return View(usuarios.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }

        }

        public ActionResult ListInactivos(string searchUser)
        {

            try
            {
                var usuarios = from c in db.USUARIOS where (c.Estado == 2) select c;

                if (!String.IsNullOrEmpty(searchUser))
                {
                    usuarios = usuarios.Where(c => c.Usuario.Contains(searchUser));

                }

                return View(usuarios.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }

        }

        [HttpPost]
        public ActionResult Acciones(String user, string Action)
        {
            if (Action == "Edit")
            {
                return RedirectToAction("Edit", new { user = user });
            }
            else if (Action == "Delete")
            {
                return RedirectToAction("Delete", new { user = user });
            }
            else
            {
                return View("List");
            }
        }

        public ActionResult Edit(string user)
        {

            var model = new USUARIOS();
            model = db.USUARIOS.SingleOrDefault(c => c.Usuario == user);
            var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            model.Roles_List = new SelectList(db.USUARIOS_TIPOS, "ROL_ID", "ROL_DESC", model.Rol);
            model.Estados_List = new SelectList(states, "ES_ID", "ES_DESCRIPCION", model.Estado);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(USUARIOS var_usuario)
        {

            if (ModelState.IsValid)
            {
                db.Entry(var_usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }


        public ActionResult Delete(String user)
        {
            USUARIOS var_usuario = db.USUARIOS.Find(user);
            if (var_usuario == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var_usuario.Estado = 0;
                db.Entry(var_usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListInactivos");
            }
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}