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
    public class PresupuestosController : Controller
    {

        private OctopusEntities1 db = new OctopusEntities1();


        // GET: /Presupuestos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRESUPUESTOS presupuestos = db.PRESUPUESTOS.Find(id);
            if (presupuestos == null)
            {
                return HttpNotFound();
            }
            return View(presupuestos);
        }

        public ActionResult Create()
        {
              var empleados = db.EMPLEADOS.Where(c => c.EMP_ESTADO == 1);
              var clientes = db.CLIENTES.Where(c => c.ES_ID == 1);
              var marcas = db.MARCAS.Where(c=>c.MAR_ID != null);
        //    //// var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
        //    //ViewBag.Rol = new SelectList(db.PRESUPUESTOS_TIPOS, "ROL_ID", "ROL_DESC");
        //    ////ViewBag.Estado = new SelectList(states, "ES_ID", "ES_DESCRIPCION");
              ViewBag.Marcas = new SelectList(marcas, "MAR_ID", "MAR_DESCRIPCION"); 
              ViewBag.Empleados = new SelectList(empleados, "EMP_ID", "EMP_NOMBRE");
              ViewBag.Clientes = new SelectList(clientes, "CLI_ID", "CLI_APELLIDO");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PRESUPUESTOS var_presupuesto, DateTime fechaPresFin)
        {

            int fecha_hoy =  int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));
            int fecha_hasta = int.Parse(string.Format("{0:yyyyMMdd}", fechaPresFin));
            if (fecha_hoy > fecha_hasta)
            {
                ModelState.AddModelError("PRE_FECHA_FIN", "Fecha Hasta: La fecha de validez debe superar la fecha actual.");
                return this.Create();
            }
            else { 
                    var_presupuesto.FEC_ID = fecha_hoy;
                    var_presupuesto.PRE_FECHA_FIN = fecha_hasta;
            }        
            var_presupuesto.ES_ID = 1; //Activo
            try
            {
                //var pre_existe = from c in db.PRESUPUESTOS select c;
                //pre_existe = pre_existe.Where(c=> c.PRE_ID.Contains(var_presupuesto.Presupuesto) )
                      db.PRESUPUESTOS.Add(var_presupuesto);
                      db.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Home", "Home");
            }
        }

        // GET: /Presupuestos/
        public ActionResult List(string searchPresupuesto, int? page)
        {

            try
            {
                var presupuestos = from c in db.PRESUPUESTOS where (c.ES_ID == 1) select c;

                if (!String.IsNullOrEmpty(searchPresupuesto))
                {
                    presupuestos.Where(v => v.MARCAS.MAR_DESCRIPCION.Contains(searchPresupuesto)
                        || v.CLIENTES.CLI_APELLIDO_NOMBRE.Contains(searchPresupuesto)
                        || v.PRE_MODELO.Contains(searchPresupuesto)
                        || v.PRE_ANIO.Contains(searchPresupuesto));

            }
                return View(presupuestos.ToList().ToPagedList(page ?? 1, 4));
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }

        }
      
        [HttpPost]
        public ActionResult Acciones(int? pre_id, string Action)
        {
            if (Action == "Edit")
            {
                return RedirectToAction("Edit", new { pre_id = pre_id });
            }
            else if (Action == "Delete")
            {
                return RedirectToAction("Delete", new { pre_id = pre_id });
            }
            else
            {
                return View("List");
            }
        }

        public ActionResult Edit(int? pre_id)
        {

            var model = new PRESUPUESTOS();
            model = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == pre_id);
            var empleados = db.EMPLEADOS.Where(c => c.EMP_ESTADO == 1 );
            var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            var clientes = db.CLIENTES.Where(c => c.ES_ID == 1);
            var marcas = db.MARCAS;

            model.Marcas_List = new SelectList(marcas, "MAR_ID", "MAR_DESCRIPCION", model.PRE_MARCA);
            model.Estados_List = new SelectList(states, "ES_ID", "ES_DESCRIPCION", model.ES_ID);
            model.Empleados_List = new SelectList(empleados, "EMP_ID", "EMP_NOMBRE", model.EMP_ID);
            model.Clientes_List = new SelectList(clientes, "CLI_ID", "CLI_NOMBRE", model.EMP_ID);
           
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PRESUPUESTOS var_presupuesto, DateTime fechaPresFin)
        {
            int fecha_hoy = var_presupuesto.FEC_ID;
            int fecha_hasta = int.Parse(string.Format("{0:yyyyMMdd}", fechaPresFin));
            if (fecha_hoy > fecha_hasta)
            {
                ModelState.AddModelError("PRE_FECHA_FIN", "Fecha Hasta: La fecha de validez debe superar la fecha actual.");
                return this.Edit(var_presupuesto.PRE_ID);
            }
            else
            {
                var_presupuesto.FEC_ID = fecha_hoy;
                var_presupuesto.PRE_FECHA_FIN = fecha_hasta;
            }
            var_presupuesto.ES_ID = 1; //Activo
            try
            {
                //var pre_existe = from c in db.PRESUPUESTOS select c;
                //pre_existe = pre_existe.Where(c=> c.PRE_ID.Contains(var_presupuesto.Presupuesto) )
                db.Entry(var_presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Home", "Home");
            }
        }


        public ActionResult Delete(int? pre_id)
        {
            PRESUPUESTOS var_presupuesto = db.PRESUPUESTOS.Find(pre_id);
            if (var_presupuesto == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var_presupuesto.ES_ID = 2;
                db.Entry(var_presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
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