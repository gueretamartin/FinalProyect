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
        private OctopusEntities1 db = new OctopusEntities1();

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

        public ActionResult Create(VEHICULOS vehsel)
        {
            var reservado = (from r in db.RESERVAS
                             where r.VEH_ID == vehsel.VEH_ID && r.RES_ESTADO != 41 //VALIDA QUE NO ESTÉ RESERVADO
                             select r.VEH_ID).FirstOrDefault();

            if (reservado == null)
            {
                RESERVAS resNew = new RESERVAS();
                resNew.VEH_ID = vehsel.VEH_ID;
                resNew.VEHICULOS = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == vehsel.VEH_ID).FirstOrDefault();
                resNew.FEC_ID_INICIO = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));

                //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_ID");
                //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID_FIN = new SelectList(db.FECHAS, "FEC_ID_FIN", "FEC_ID");

                var condiciones = (from c in db.CLIENTES
                                   select c.TC_ID).Distinct();
                //ViewBag.TC_ID = new SelectList(condiciones);
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                var clientes_ri = (from c in db.CLIENTES
                                   orderby c.CLI_RI_RAZONSOCIAL ascending
                                   where c.TC_ID == 2
                                   select c).Distinct();
                ViewBag.CLI_RI_ID = new SelectList(clientes_ri, "CLI_ID", "CLI_RZ_CUIT");

                //CLIENTES FINALES
                var clientes_cf = (from c in db.CLIENTES
                                   orderby c.CLI_APELLIDO
                                   where c.TC_ID == 1
                                   select c).Distinct();
                ViewBag.CLI_CF_ID = new SelectList(clientes_cf, "CLI_ID", "CLI_APELLIDO_CUIL");

                return View(resNew);
            }
            else
                return RedirectToAction("List", "Vehiculos");
        }

        // POST: /Reservas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RESERVAS reservas, DateTime fechaResFin, int? CLI_RI_ID, int? CLI_CF_ID, int TC_ID, int SUCURSALES_ID, int EMPLEADO_ID)
        {
            
            if (TC_ID == 1 && CLI_CF_ID != null)
            {
                reservas.CLI_ID = (from c in db.CLIENTES where c.CLI_ID == CLI_CF_ID select c.CLI_ID).FirstOrDefault();
                reservas.CLIENTES = (from c in db.CLIENTES where c.CLI_ID == CLI_CF_ID select c).FirstOrDefault();
            }
            else if (TC_ID == 2 && CLI_RI_ID != null)
            {
                reservas.CLI_ID = (from c in db.CLIENTES where c.CLI_ID == CLI_RI_ID select c.CLI_ID).FirstOrDefault();
                reservas.CLIENTES = (from c in db.CLIENTES where c.CLI_ID == CLI_RI_ID select c).FirstOrDefault();
            }
            else
            {

                RESERVAS resNew = new RESERVAS();
                resNew.VEH_ID = reservas.VEH_ID;
                VEHICULOS vehSel = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == resNew.VEH_ID).FirstOrDefault();
                resNew.VEHICULOS = vehSel;
                resNew.FEC_ID_INICIO = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));

                //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_ID");
                //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID_FIN = new SelectList(db.FECHAS, "FEC_ID_FIN", "FEC_ID");

                var condiciones = (from c in db.CLIENTES
                                   select c.TC_ID).Distinct();
                //ViewBag.TC_ID = new SelectList(condiciones);
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                var clientes_ri = (from c in db.CLIENTES
                                   orderby c.CLI_RI_RAZONSOCIAL ascending
                                   where c.TC_ID == 2
                                   select c).Distinct();
                ViewBag.CLI_RI_ID = new SelectList(clientes_ri, "CLI_ID", "CLI_RZ_CUIT");

                //CLIENTES FINALES
                var clientes_cf = (from c in db.CLIENTES
                                   orderby c.CLI_APELLIDO
                                   where c.TC_ID == 1
                                   select c).Distinct();
                ViewBag.CLI_CF_ID = new SelectList(clientes_cf, "CLI_ID", "CLI_APELLIDO_CUIL");
                ModelState.AddModelError("CLI_ID", "LOS CAMPOS 'CONDICION IVA' Y 'DNI/CUIT' SON OBLIGATORIOS");

                return RedirectToAction("Create", "Reservas", vehSel);
            }

            reservas.FECHAS = db.FECHAS.Find(reservas.FEC_ID_INICIO);

            var FechaFinSel = from f in db.FECHAS
                            where f.FEC_FECHA == fechaResFin
                            select f.FEC_ID;
            reservas.FEC_ID_FIN = FechaFinSel.FirstOrDefault();
            reservas.FECHAS1 = db.FECHAS.Find(reservas.FEC_ID_FIN);

            reservas.VEHICULOS = db.VEHICULOS.Find(reservas.VEH_ID);

            reservas.SUC_ID = SUCURSALES_ID;
            reservas.SUCURSALES = db.SUCURSALES.Find(reservas.SUC_ID);

            reservas.EMP_ID = EMPLEADO_ID;
            reservas.EMPLEADOS = db.EMPLEADOS.Find(reservas.EMP_ID);

            reservas.RES_ESTADO = 41;

            if (ModelState.IsValid)
            {
                db.RESERVAS.Add(reservas);
                db.SaveChanges();
                return RedirectToAction("List");
            } 

            return View(reservas);
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
