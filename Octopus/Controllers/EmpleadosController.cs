using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using PagedList;
using PagedList.Mvc;

namespace Octopus.Controllers
{
    public class EmpleadosController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        // GET: /Empleados/
        public ActionResult List(string searchEmpleado, int? page)
        {
            var empleados = from c in db.EMPLEADOS
                            select c;

            empleados = empleados.Where(a => a.EMP_ESTADO == 1);

            if (!String.IsNullOrEmpty(searchEmpleado))
            {
                empleados = empleados.Where(c => c.EMP_NOMBRE.Contains(searchEmpleado)
                    || c.EMP_APELLIDO.Contains(searchEmpleado)
                    || c.EMP_DNI.Contains(searchEmpleado));
            }
            return View(empleados.ToList().ToPagedList(page ?? 1, 4));
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
            ViewBag.Sucursal = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
            ViewBag.Cargos = new SelectList(db.CARGOS, "CARGO_ID", "CARGO_DESC");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EMPLEADOS empleados)
        {
            //CHEQUEA SI EL DOCUMENTO YA FUE CARGADO


            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(empleados.EMP_DNI))
                {
                    var empleado = db.EMPLEADOS.SingleOrDefault(a => a.EMP_DNI == empleados.EMP_DNI);

                    if (empleado != null)
                    {
                        ModelState.AddModelError("EMP_DNI", "EMPLEADO (DNI): El Empleado ingresado ya existe.");
                        return this.Create();
                    }

                }
                else
                {
                    ModelState.AddModelError("EMP_DNI", "EMPLEADO (DNI): Por favor, ingrese el DNI del Empleado.");
                    return this.Create();
                }


                empleados.EMP_ESTADO = 1;
                db.EMPLEADOS.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(empleados);
        }

        public ActionResult Edit(int EMP_ID)
        {
            try
            {
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", db.EMPLEADOS.SingleOrDefault(e => e.EMP_ID == EMP_ID).SUC_ID);
                ViewBag.CARGO_ID = new SelectList(db.CARGOS, "CARGO_ID", "CARGO_DESC", db.EMPLEADOS.SingleOrDefault(e => e.EMP_ID == EMP_ID).EMP_CARGO);
                
                EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.EMP_ID == EMP_ID);
                return View(empleado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Edit an Employee 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EMPLEADOS empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                var empleado = db.EMPLEADOS.Single(u => u.EMP_ID == empleados.EMP_ID);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
            ViewBag.Cargos = new SelectList(db.CARGOS, "CARGO_ID", "CARGO_DESC", empleados.EMP_CARGO);
           
            return View(empleados);
        }


        //public ActionResult Delete(String user)
        //{
        //    USUARIOS var_usuario = db.USUARIOS.Find(user);
        //    if (var_usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var_usuario.Estado = 0;
        //        db.Entry(var_usuario).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("ListInactivos");
        //    }
        //    return RedirectToAction("List");
        //}

        // POST: /Vehiculos/Delete/5
        public ActionResult Delete(int emp_id)
        {
          
            EMPLEADOS var_empleado = db.EMPLEADOS.Find(emp_id);
            if (var_empleado == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var_empleado.EMP_ESTADO = 2;
                if (var_empleado.USUARIOS!=null)
                {
                    var_empleado.USUARIOS.Estado = 2;
                }
                db.Entry(var_empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
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

        // Menú en el detalle 

        [HttpPost]
        public ActionResult Acciones(int emp_id, string Action)
        {
            if (Action == "Edit")
            {
                return RedirectToAction("Edit", new { emp_id = emp_id });
            }
            else if (Action == "Delete")
            {
                return Delete(emp_id);
            }
            else
            {
                return View("List");
            }
        }

    }
}
