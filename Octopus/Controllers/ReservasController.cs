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
        // GET: /Reservas/Details/5
        public ActionResult Details(int res_id = 0)
        {
            if (res_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS reservas = db.RESERVAS.Include(r => r.CLIENTES).Include(r => r.ESTADOS).Include(r => r.EMPLEADOS).Include(r => r.FECHAS).Include(r => r.VEHICULOS).SingleOrDefault(r => r.RES_ID == res_id);
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
        public ActionResult Create(RESERVAS reservas, DateTime? fechaResFin, int? CLI_RI_ID, int? CLI_CF_ID, int? TC_ID, int? SUCURSALES_ID, int? EMPLEADO_ID)
        {

            RESERVAS resNew = new RESERVAS();
            resNew.VEH_ID = reservas.VEH_ID;
            VEHICULOS vehSel = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == resNew.VEH_ID).FirstOrDefault();
            resNew.VEHICULOS = vehSel;
            resNew.FEC_ID_INICIO = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));

            /*
            bool error = false;
            if (fechaResFin == null)
            {

                ModelState.AddModelError("FEC_ID_FIN", "FECHA RESERVA: Seleccione la fecha de vencimiento");
                error = true;
            }
            if (fechaResFin != null  )
            {
                string anio = fechaResFin.ToString().Substring(6, 4);
                string mes = fechaResFin.ToString().Substring(3, 2);
                string dia = fechaResFin.ToString().Substring(0, 2);
                string fecha = anio + mes + dia;
                int fechaf = Convert.ToInt32(fecha);
                if (resNew.FEC_ID_INICIO > fechaf)
                { 
                ModelState.AddModelError("FEC_ID_FIN", "FECHA RESERVA: La fecha de vencimiento debe ser mayor o igual a la fecha de hoy");
                error = true;
                }
            }
            if (TC_ID == null)
            {
                ModelState.AddModelError("TC_ID", "TIPO IVA: Seleccione el Tipo de IVA");
                error = true;
            }

            if (CLI_CF_ID == null && CLI_RI_ID == null)
            {
                ModelState.AddModelError("CLI_ID", "CLIENTE: Seleccione el Cliente");
                error = true;
            }

            if (SUCURSALES_ID == null )
            {
                ModelState.AddModelError("SUC_ID", "SUCURSAL: Seleccione la Sucursal");
                error = true;
            }

            if (EMPLEADO_ID == null)
            {
                ModelState.AddModelError("EMP_ID", "EMPLEADO: Seleccione el Empleado");
                error = true;
            }
            if (reservas.RES_SENIA == null)
            {
                ModelState.AddModelError("RES_SENIA", "SEÑA: Ingrese un valor de seña");
                error = true;
            }

            if(reservas.RES_VALOR_PACTADO ==null)
            {
                ModelState.AddModelError("RES_VALOR_PACTADO", "VALOR PACTADO: Ingrese el valor pactado para el vehículo");
                error = true;
            }

            if (error==true)
            {
                return this.Create(vehSel);
            }
              */

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
                //return RedirectToAction("List");
            }

            
            try
            {
                return RedirectToAction("UpdateState", "Vehiculos", new { veh_id = reservas.VEH_ID, ESTADO_ID = 32, operation = "Reserve" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult List(string searchReserve)
        {
            try
            {
                var reservas = db.RESERVAS.Include(r => r.CLIENTES).Include(r => r.EMPLEADOS).Include(r => r.FECHAS).Include(r => r.SUCURSALES).Where(r => r.RES_ESTADO == 41);
                if (!String.IsNullOrEmpty(searchReserve))
                {
                    reservas = reservas.Where(r => r.VEHICULOS.VEH_PATENTE.Contains(searchReserve)
                        || r.VEHICULOS.MARCAS.MAR_DESCRIPCION.Contains(searchReserve)
                        || r.VEHICULOS.VEH_MODELO.Contains(searchReserve)
                        || r.CLIENTES.CLI_APELLIDO.Contains(searchReserve)
                        || r.CLIENTES.CLI_CUIL.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_CUIT.Contains(searchReserve)
                        || r.CLIENTES.CLI_DOC.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_RAZONSOCIAL.Contains(searchReserve)
                        );
                }

                return View(reservas.ToList());
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public ActionResult ListVencidas(string searchReserve)
        {
            try
            {
                var reservas = db.RESERVAS.Include(r => r.CLIENTES).Include(r => r.EMPLEADOS).Include(r => r.FECHAS).Include(r => r.SUCURSALES).Where(r => r.RES_ESTADO == 43);
                if (!String.IsNullOrEmpty(searchReserve))
                {
                    reservas = reservas.Where(r => r.VEHICULOS.VEH_PATENTE.Contains(searchReserve)
                        || r.VEHICULOS.MARCAS.MAR_DESCRIPCION.Contains(searchReserve)
                        || r.VEHICULOS.VEH_MODELO.Contains(searchReserve)
                        || r.CLIENTES.CLI_APELLIDO.Contains(searchReserve)
                        || r.CLIENTES.CLI_CUIL.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_CUIT.Contains(searchReserve)
                        || r.CLIENTES.CLI_DOC.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_RAZONSOCIAL.Contains(searchReserve)
                        );
                }

                return View(reservas.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ListCanceladas(string searchReserve)
        {
            try
            {
                var reservas = db.RESERVAS.Include(r => r.CLIENTES).Include(r => r.EMPLEADOS).Include(r => r.FECHAS).Include(r => r.SUCURSALES).Where(r => r.RES_ESTADO == 42);
                if (!String.IsNullOrEmpty(searchReserve))
                {
                    reservas = reservas.Where(r => r.VEHICULOS.VEH_PATENTE.Contains(searchReserve)
                        || r.VEHICULOS.MARCAS.MAR_DESCRIPCION.Contains(searchReserve)
                        || r.VEHICULOS.VEH_MODELO.Contains(searchReserve)
                        || r.CLIENTES.CLI_APELLIDO.Contains(searchReserve)
                        || r.CLIENTES.CLI_CUIL.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_CUIT.Contains(searchReserve)
                        || r.CLIENTES.CLI_DOC.Contains(searchReserve)
                        || r.CLIENTES.CLI_RI_RAZONSOCIAL.Contains(searchReserve)
                        );
                }

                return View(reservas.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: /Reservas/Edit/5
        public ActionResult Edit(int? res_id)
        {
            if (res_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS reservas = db.RESERVAS.Find(res_id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", reservas.CLI_ID);
            //ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", reservas.EMP_ID);
            ViewBag.FEC_ID_FIN = new SelectList(db.FECHAS, "FEC_ID", "FEC_NOMBREDIA", reservas.FEC_ID_FIN);
            //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MARCA", reservas.VEH_ID);
            return View(reservas);
        }

        // POST: /Reservas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RESERVAS reservas, DateTime fechaResFin)
        {
            bool error = false;
            if (fechaResFin == null)
            {

                ModelState.AddModelError("FEC_ID_FIN", "FECHA RESERVA: Seleccione la fecha de vencimiento");
                error = true;
            }
            if (fechaResFin != null)
            {
                string anio = fechaResFin.ToString().Substring(6, 4);
                string mes = fechaResFin.ToString().Substring(3, 2);
                string dia = fechaResFin.ToString().Substring(0, 2);
                string fecha = anio + mes + dia;
                int fechaf = Convert.ToInt32(fecha);
                if (reservas.FEC_ID_INICIO > fechaf)
                {
                    ModelState.AddModelError("FEC_ID_FIN", "FECHA RESERVA: La fecha de vencimiento debe ser mayor o igual a la fecha de hoy");
                    error = true;
                }
            }
          
            if (reservas.RES_SENIA == null)
            {
                ModelState.AddModelError("RES_SENIA", "SEÑA: Ingrese un valor de seña");
                error = true;
            }

            if (reservas.RES_VALOR_PACTADO == null)
            {
                ModelState.AddModelError("RES_VALOR_PACTADO", "VALOR PACTADO: Ingrese el valor pactado para el vehículo");
                error = true;
            }

            if (error == true)
            {
                return this.Edit(reservas.RES_ID);
            }
            //ASIGNA ESTADO DE RESERVA
            if (fechaResFin >= DateTime.Now) { reservas.RES_ESTADO = 41; }
            else { reservas.RES_ESTADO = 43; };

            reservas.FEC_ID_FIN = (from f in db.FECHAS
                                  where f.FEC_FECHA == fechaResFin
                                  select f.FEC_ID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {res_id = reservas.RES_ID});
            }
            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", reservas.CLI_ID);
            //ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", reservas.EMP_ID);
            ViewBag.FEC_ID_FIN = new SelectList(db.FECHAS, "FEC_ID", "FEC_NOMBREDIA", reservas.FEC_ID_FIN);
            //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MARCA", reservas.VEH_ID);
            return RedirectToAction("Details", reservas.RES_ID);
        }

        
        // POST: /Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int res_id)
        {
            RESERVAS reservas = db.RESERVAS.Find(res_id);
            reservas.RES_ESTADO = 42;
            db.Entry(reservas).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListCanceladas");
        }

        [HttpPost]
        public ActionResult Acciones(int res_id, string Action)
        {
            try
            {

                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { res_id = res_id });
                }
                else if (Action == "Delete")
                {
                    return Delete(res_id);
                }
                else
                {
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //     return RedirectToAction("List");


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
