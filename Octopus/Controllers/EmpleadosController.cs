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
     
        #region DesarrolloMartin
        public ActionResult List(string searchEmpleado, int? page)
        {
             var empleados = from c in db.EMPLEADOS
                           select c;

           
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

       

        #endregion


        // GET: /Empleados/Create
        public ActionResult Create()
        {
            
            //var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            //ViewBag.Estado = new SelectList(states, "ES_ID", "ES_DESCRIPCION");
            ViewBag.Sucursal = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EMPLEADOS empleados)
        {
            //CHEQUEA SI EL DOCUMENTO YA FUE CARGADO
           

            if (ModelState.IsValid)
            {
             //   var empleado = db.EMPLEADOS.Single(u => u.EMP_ID == empleados.EMP_ID);
              //  empleados.EMP_APELLIDO_NOMBRE = empleados.EMP_APELLIDO + " " + empleados.EMP_NOMBRE;
                //CleanClient(clientes);
                empleados.EMP_ESTADO = 1;
                db.EMPLEADOS.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(empleados);
        }



        // POST: /Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="EMP_ID,SUC_ID,EMP_CODPOSTAL,EMP_NOMBRE,EMP_APELLIDO,EMP_EMAIL,EMP_USER,EMP_PASSWORD,EMP_TIPO,EMP_DIRECCION,EMP_TELEFONO,EMP_DNI,EMP_APELLIDO_NOMBRE")] EMPLEADOS empleados)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.EMPLEADOS.Add(empleados);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
        //    return View(empleados);
        //}

        public ActionResult Edit(int EMP_ID)
        {
            try
            {
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", db.EMPLEADOS.SingleOrDefault(e => e.EMP_ID == EMP_ID).SUC_ID);
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
               // empleado.EMP_APELLIDO_NOMBRE = empleados.EMP_APELLIDO + " " + empleados.EMP_NOMBRE;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", empleados.SUC_ID);
            return View(empleados);
        }


        // POST: /Vehiculos/Delete/5
        
        public ActionResult Delete(int emp_id)
        {
            EMPLEADOS empleado = db.EMPLEADOS.Find(Session["Usuario"]);
            db.EMPLEADOS.Remove(empleado);
            db.SaveChanges();
            return RedirectToAction("List");
        }

 
        //// GET: /Empleados/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EMPLEADOS empleados = db.EMPLEADOS.Find(id);
        //    if (empleados == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(empleados);
        //}

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
