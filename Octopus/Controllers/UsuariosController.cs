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

        //
        // GET: /USUARIOS/

        public ActionResult Index()
        {
            return View(db.USUARIOS.ToList());
        }

        //
        // GET: /USUARIOS/Details/5

        public ActionResult Details(String user)
        {
            USUARIOS USUARIOS = db.USUARIOS.Find(user);
            if (USUARIOS == null)
            {
                return HttpNotFound();
            }
           // Session["CONDICIONIVA"] = USUARIOS.TC_ID.ToString();
            
            return View(USUARIOS);
        }

        //
        // GET: /USUARIOS/Create

        public ActionResult Create()
        {
            //USUARIOS user = @ViewBag.Usuario;
            //return RedirectToAction("Create");
            return View();
            //return View(user);
        }

        //
        // POST: /USUARIOS/Create

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(USUARIOS USUARIOS)
        {
            //CHEQUEA SI EL DOCUMENTO YA FUE CARGADO
          //  if ((db.USUARIOS.Any(c => c.CLI_DOC.Equals(USUARIOS.CLI_DOC)) && !String.IsNullOrEmpty(USUARIOS.CLI_DOC))
            //    || (db.USUARIOS.Any(c => c.CLI_RI_CUIT.Equals(USUARIOS.CLI_RI_CUIT)) && !String.IsNullOrEmpty(USUARIOS.CLI_RI_CUIT)))
            
                //ModelState.AddModelError("CLI_DOC", "EL DNI/CUIT INGRESADO YA EXISTE");
                //return View("Create");
            

            //CHEQUEA QUE HAYA COMPLETADO LO NECESARIO EN ALGUN TIPO DE CONDICIÓN IVA
            //if((USUARIOS.TC_ID.Equals("CONSUMIDOR_FINAL") && (USUARIOS.TD_ID == null || USUARIOS.CLI_DOC == null))
            //    ||
            //   (USUARIOS.TC_ID.Equals("RESPONSABLE_INSCRIPTO") && USUARIOS.CLI_RI_CUIT == null))
            //{
            //    ModelState.AddModelError("CLI_DOC", "DEBE REGISTRAR ALGÚN DOCUMENTO/CUIT");
            //    return View("Create");
            //}

            if (ModelState.IsValid)
            {
                db.USUARIOS.Add(USUARIOS);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(USUARIOS);
        }

       

        /*
        public ActionResult List()
        {
            return View(db.USUARIOS.ToList());
        }
         * */


        public ActionResult List(string searchClient)
        {
            var USUARIOS = from c in db.USUARIOS
                           select c;

            //var USUARIOS = db.USUARIOS;

            //if (!String.IsNullOrEmpty(searchClient))
            //{
            //    USUARIOS = USUARIOS.Where(c => c.CLI_NOMBRE.Contains(searchClient)
            //        || c.CLI_CF_APELLIDO.Contains(searchClient)
            //        || c.CLI_DOC.Contains(searchClient));
            //}
            return View(USUARIOS.ToList());
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

        //
        // GET: /USUARIOS/Edit/5

        public ActionResult Edit(string user)
        {
            try
            {
                USUARIOS cliente = db.USUARIOS.SingleOrDefault(c => c.Usuario == user);
                return View(cliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // POST: /USUARIOS/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(USUARIOS USUARIOS)
        {
           // USUARIOS.TC_ID = Session["CONDICIONIVA"].ToString();
            if (ModelState.IsValid)
            {
                db.Entry(USUARIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(USUARIOS);
        }

        //
        // GET: /USUARIOS/Delete/5

        public ActionResult Delete(String user)
        {
            USUARIOS USUARIOS = db.USUARIOS.Find(user);
            if (USUARIOS == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.USUARIOS.Remove(USUARIOS);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }

        //
        // POST: /USUARIOS/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USUARIOS USUARIOS = db.USUARIOS.Find(id);
            db.USUARIOS.Remove(USUARIOS);
            db.SaveChanges();
            return RedirectToAction("List");
        }
          */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(USUARIOS USUARIOS)
        {
            if (ModelState.IsValid)
            {
                USUARIOS cliact = db.USUARIOS.Find(USUARIOS.Usuario);
                db.USUARIOS.Remove(cliact);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(USUARIOS);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}