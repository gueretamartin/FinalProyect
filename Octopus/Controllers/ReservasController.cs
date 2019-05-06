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
    public class ReservasController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        // GET: /Reservas/
        public ActionResult Index()
        {
            var reservas = db.RESERVAS.Include(r => r.CLIENTES).Include(r => r.EMPLEADOS).Include(r => r.FECHAS).Include(r => r.VEHICULOS);
            return View(reservas.ToList());
        }

        // GET: /Reservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS reservas = db.RESERVAS.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }


        // POST: /Reservas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RESERVAS reservas, DateTime fecha, DateTime fechaResFin)
        {

            ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", reservas.CLI_ID);
            ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", reservas.EMP_ID);
            ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_NOMBREDIA", reservas.FEC_ID_INICIO);
            ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MARCA", reservas.VEH_ID);
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", reservas.SUC_ID);
            ViewBag.FEC_ID_FIN = new SelectList(db.SUCURSALES, "FEC_ID_FIN", "SUC_DESCRIP", reservas.FEC_ID_FIN);

            var FechaSel = from f in db.FECHAS
                           where f.FEC_FECHA == fecha
                           select f.FEC_ID;
            reservas.FEC_ID_INICIO = FechaSel.FirstOrDefault();

            var FechaFinSel = from f in db.FECHAS
                           where f.FEC_FECHA == fechaResFin
                           select f.FEC_ID;
            reservas.FEC_ID_FIN = FechaFinSel.FirstOrDefault();

            reservas.RES_ESTADO = true;

            if (ModelState.IsValid)
            {
                db.RESERVAS.Add(reservas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reservas);
        }

        
        public ActionResult Create(VEHICULOS vehsel)
        {
            var reservado = (from r in db.RESERVAS
                              where r.VEH_ID == vehsel.VEH_ID && r.RES_ESTADO == true
                                 select r.VEH_ID).FirstOrDefault();

            if (reservado == null)
            {
                RESERVAS resNew = new RESERVAS();

                ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_ID");
                ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID_FIN = new SelectList(db.FECHAS, "FEC_ID_FIN", "FEC_ID");

                var condiciones = (from c in db.CLIENTES
                                   select c.TC_ID).Distinct();
                //ViewBag.TC_ID = new SelectList(condiciones);
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                var clientes_ri = (from c in db.CLIENTES
                                where c.TC_ID == 2
                                select c.CLI_RI_CUIT).Distinct();
                ViewBag.CLI_RI_ID = new SelectList(clientes_ri);

                //CLIENTES FINALES
                var clientes_cf = (from c in db.CLIENTES
                                    where c.TC_ID == 1
                                    select c.CLI_DOC).Distinct();
                ViewBag.CLI_CF_ID = new SelectList(clientes_cf);

                return View(resNew);
            }
            else
                return RedirectToAction("List","Vehiculos");
        }



        // GET: /Reservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS reservas = db.RESERVAS.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", reservas.CLI_ID);
            ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", reservas.EMP_ID);
            ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID", "FEC_NOMBREDIA", reservas.FEC_ID_INICIO);
            ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MARCA", reservas.VEH_ID);
            return View(reservas);
        }

        // POST: /Reservas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RES_ID,VEH_ID,EMP_ID,RES_SENIA,RES_VALOR_PACTADO,RES_INICIO,RES_FIN,RES_ESTADO,FEC_ID,CLI_ID")] RESERVAS reservas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", reservas.CLI_ID);
            ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", reservas.EMP_ID);
            ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID", "FEC_NOMBREDIA", reservas.FEC_ID_INICIO);
            ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MARCA", reservas.VEH_ID);
            return View(reservas);
        }

        // GET: /Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS reservas = db.RESERVAS.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        // POST: /Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RESERVAS reservas = db.RESERVAS.Find(id);
            db.RESERVAS.Remove(reservas);
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
