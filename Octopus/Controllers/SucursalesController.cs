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
    public class SucursalesController : Controller
    {
        private OctopusEntities1 db = new OctopusEntities1();

        //
        // GET: /Sucursales/

        public ActionResult Index()
        {
            return View(db.SUCURSALES.ToList());
        }

        //
        // GET: /Sucursales/Details/5

        public ActionResult Details(int id = 0)
        {
            SUCURSALES sucursales = db.SUCURSALES.Find(id);
            if (sucursales == null)
            {
                return HttpNotFound();
            }
          //  Session["CONDICIONIVA"] = sucursales.CLI_CONDICIONIVA.ToString();
            
            return View(sucursales);
        }

        //
        // GET: /Sucursales/Create

        public ActionResult Create()
        {
            //USUARIOS user = @ViewBag.Usuario;
            //return RedirectToAction("Create");
            return View();
            //return View(user);
        }

        //
        // POST: /Sucursales/Create

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SUCURSALES sucursales)
        {
        

            if (ModelState.IsValid)
            {
              
                db.SUCURSALES.Add(sucursales);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(sucursales);
        }

 
        /*
        public ActionResult List()
        {
            return View(db.SUCURSALES.ToList());
        }
         * */


        public ActionResult List(string searchSucursal)
        {
            var sucursales = from c in db.SUCURSALES
                           select c;

            //var sucursales = db.SUCURSALES;

            if (!String.IsNullOrEmpty(searchSucursal))
            {
                
            }
            return View(sucursales.ToList());
        }


        [HttpPost]
        public ActionResult Acciones(int suc_id, string Action)
        {
            if (Action == "Edit")
            {
                return RedirectToAction("Edit", new { suc_id = suc_id });
            }
            else if (Action == "Delete")
            {
                return RedirectToAction("Delete", new { suc_id = suc_id });
            }
            else
            {
                return View("List");
            }
        }

        //
        // GET: /Sucursales/Edit/5

        public ActionResult Edit(int suc_id)
        {
            try
            {
                SUCURSALES sucursal = db.SUCURSALES.SingleOrDefault(c => c.SUC_ID == suc_id);
                return View(sucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // POST: /Sucursales/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SUCURSALES sucursales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sucursales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(sucursales);
        }

        //
        // GET: /Sucursales/Delete/5

        public ActionResult Delete(int cli_id)
        {
            SUCURSALES sucursales = db.SUCURSALES.Find(cli_id);
            if (sucursales == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.SUCURSALES.Remove(sucursales);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }

        //
        // POST: /Sucursales/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUCURSALES sucursales = db.SUCURSALES.Find(id);
            db.SUCURSALES.Remove(sucursales);
            db.SaveChanges();
            return RedirectToAction("List");
        }
          */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SUCURSALES sucursales)
        {
            if (ModelState.IsValid)
            {
                SUCURSALES cliact = db.SUCURSALES.Find(sucursales.SUC_ID);
                db.SUCURSALES.Remove(cliact);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(sucursales);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}