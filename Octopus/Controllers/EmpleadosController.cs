using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;

namespace Octopus.Controllers
{
    public class EmpleadosController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        // GET: /Empleados/
        public ActionResult Index()
        {
            var empleados = db.EMPLEADOS.Include(e => e.SUCURSALES);
            return View(empleados.ToList());
        }

        // GET: /Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS empleados = db.EMPLEADOS.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: /Empleados/Create
        public ActionResult Create()
        {
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
            return View();
        }

        // POST: /Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EMP_ID,SUC_ID,EMP_CODPOSTAL,EMP_NOMBRE,EMP_APELLIDO,EMP_EMAIL,EMP_USER,EMP_PASSWORD,EMP_TIPO,EMP_DIRECCION,EMP_TELEFONO,EMP_DNI,EMP_APELLIDO_NOMBRE")] EMPLEADOS empleados)
        {
            if (ModelState.IsValid)
            {
                db.EMPLEADOS.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
            return View(empleados);
        }

        // GET: /Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS empleados = db.EMPLEADOS.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
            return View(empleados);
        }

        // POST: /Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EMP_ID,SUC_ID,EMP_CODPOSTAL,EMP_NOMBRE,EMP_APELLIDO,EMP_EMAIL,EMP_USER,EMP_PASSWORD,EMP_TIPO,EMP_DIRECCION,EMP_TELEFONO,EMP_DNI,EMP_APELLIDO_NOMBRE")] EMPLEADOS empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
            return View(empleados);
        }

        // GET: /Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADOS empleados = db.EMPLEADOS.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: /Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EMPLEADOS empleados = db.EMPLEADOS.Find(id);
            db.EMPLEADOS.Remove(empleados);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
