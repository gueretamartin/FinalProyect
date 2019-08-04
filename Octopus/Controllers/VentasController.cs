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
    public class VentasController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        public ActionResult Edit(int? ven_id)
        {

            var model = new VENTAS();
            model = db.VENTAS.SingleOrDefault(c => c.VEN_ID == ven_id);

            //venNew.VEHICULOS = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == vehsel.VEH_ID).FirstOrDefault();
            //venNew.FEC_ID = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));
            int valor = Convert.ToInt32(model.VEN_VALOR);
            model.VEN_VALOR = valor;
            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
            ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE",model.EMP_ID);
            //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
            ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP",model.SUC_ID);
            ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_ID",model.FEC_ID);
            ViewBag.MAR_ID = new SelectList(db.MARCAS, "MAR_ID", "MAR_DESCRIPCION",model.VEN_VEH_MAR_ID_USADO);

            var condiciones = (from c in db.CLIENTES
                               select c.TC_ID).Distinct();
            //ViewBag.TC_ID = new SelectList(condiciones);
            ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION",model.CLIENTES.TIPO_CLIENTE);

            
            // CLIENTES RI
            var clientes_ri = (from c in db.CLIENTES
                               orderby c.CLI_RI_RAZONSOCIAL ascending
                               where c.TC_ID == 2
                               select c).Distinct();
            ViewBag.CLI_RI_ID = new SelectList(clientes_ri, "CLI_ID", "CLI_RZ_CUIT",model.CLIENTES.CLI_ID);

            //CLIENTES FINALES
            var clientes_cf = (from c in db.CLIENTES
                               orderby c.CLI_APELLIDO
                               where c.TC_ID == 1
                               select c).Distinct();
            ViewBag.CLI_CF_ID = new SelectList(clientes_cf, "CLI_ID", "CLI_APELLIDO_CUIL", model.CLIENTES.CLI_ID);

            //var empleados = db.EMPLEADOS.Where(c => c.EMP_ESTADO == 1);
            ////var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            //var clientes = db.CLIENTES.Where(c => c.ES_ID == 1);
            //var marcas = db.MARCAS;

            //model.Marcas_List = new SelectList(marcas, "MAR_ID", "MAR_DESCRIPCION", model.PRE_MARCA);
            //model.Estados_List = new SelectList(states, "ES_ID", "ES_DESCRIPCION", model.ES_ID);
            ////model.Empleados_List = new SelectList(empleados, "EMP_ID", "EMP_APELLIDO_NOMBRE", model.EMP_ID);
            //model.Clientes_List = new SelectList(clientes, "CLI_ID", "CLI_APELLIDO_NOMBRE", model.EMP_ID);

            return View(model);

        }

        public ActionResult List(string searchVenta, int? page)
        {

            try
            {
                var ventas = from c in db.VENTAS select c;
                ventas = ventas.Where(c => c.VEN_ESTADO == 51);
                if (!String.IsNullOrEmpty(searchVenta))
                {
                    ventas = ventas.Where(c =>
                           c.VEHICULOS.VEH_PATENTE.Contains(searchVenta)
                          || c.VEHICULOS.MARCAS.MAR_DESCRIPCION.Contains(searchVenta)
                          || c.VEHICULOS.VEH_MODELO.Contains(searchVenta)
                          || c.CLIENTES.CLI_APELLIDO.Contains(searchVenta)
                          || c.CLIENTES.CLI_NOMBRE.Contains(searchVenta)
                          || c.EMPLEADOS.EMP_APELLIDO.Contains(searchVenta)
                          || c.EMPLEADOS.EMP_NOMBRE.Contains(searchVenta)
                           );

                }
                return View(ventas.ToList().ToPagedList(page ?? 1, 6));
            }
            catch (Exception e)
            {
                return RedirectToAction("Home", "Home");
            }

        }

        // GET: /Ventas/
        public ActionResult Index()
        {
            var ventas = db.VENTAS.Include(v => v.CLIENTES).Include(v => v.EMPLEADOS).Include(v => v.FECHAS).Include(v => v.VEHICULOS);
            return View(ventas.ToList());
        }

        // GET: /Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS ventas = db.VENTAS.Find(id);
            if (ventas == null)
            {
                return HttpNotFound();
            }
            return View(ventas);
        }

        // GET: /Ventas/Create
        public ActionResult Create(VEHICULOS vehsel)
        {
            if (vehsel.VEH_ID !=0)
            { 
            var vendido = (from v in db.VENTAS
                             where v.VEH_ID == vehsel.VEH_ID && v.VEN_ESTADO != 51 //VALIDA QUE NO ESTÉ VENDIDO
                             select v.VEH_ID).FirstOrDefault();

            if (vendido == null)
            {
                VENTAS venNew = new VENTAS();
                venNew.VEH_ID = vehsel.VEH_ID;
                venNew.VEHICULOS = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == vehsel.VEH_ID).FirstOrDefault();
                //venNew.FEC_ID = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));

                //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_ID");
                ViewBag.MAR_ID = new SelectList(db.MARCAS, "MAR_ID", "MAR_DESCRIPCION");

                var condiciones = (from c in db.CLIENTES
                                   select c.TC_ID).Distinct();
                //ViewBag.TC_ID = new SelectList(condiciones);
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                // CLIENTES RI
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

                return View(venNew);
            }
            else
                return RedirectToAction("List", "Vehiculos");
            }
            else return RedirectToAction("Home", "Home");

        }

        // POST: /Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VENTAS ventas, DateTime? fecha ,int? MAR_ID,int? CLI_RI_ID, int? CLI_CF_ID, int? TC_ID, int? SUCURSALES_ID, int? EMPLEADO_ID)
        {
            
            VENTAS venNew = new VENTAS();
            venNew.VEH_ID = ventas.VEH_ID;
            VEHICULOS vehSel = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == venNew.VEH_ID).FirstOrDefault();
            venNew.VEHICULOS = vehSel;
           
            //venNew.FEC_ID = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));

            bool error = false;
            if (fecha == null)
            {

                ModelState.AddModelError("FEC_ID", "FECHA VENTA: Seleccione la fecha de venta");
                error = true;
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

            if (SUCURSALES_ID == null)
            {
                ModelState.AddModelError("SUC_ID", "SUCURSAL: Seleccione la Sucursal");
                error = true;
            }

            if (EMPLEADO_ID == null)
            {
                ModelState.AddModelError("EMP_ID", "EMPLEADO: Seleccione el Empleado");
                error = true;
            }
            if (ventas.VEN_VALOR == null)
            {
                ModelState.AddModelError("VEN_VALOR", "VALOR: Ingrese un valor de seña");
                error = true;
            }
            
            if (ventas.VEN_VEH_ENT_USADO == null)
            {
                ModelState.AddModelError("VEN_VEH_ENT_USADO", "ENTREGA USADO: Debe seleccionar un valor");
                error = true;
            }

            if (error == true)
            {
                return this.Create(vehSel);
            }

            if (TC_ID == 1 && CLI_CF_ID != null)
            {
                ventas.CLI_ID = (from c in db.CLIENTES where c.CLI_ID == CLI_CF_ID select c.CLI_ID).FirstOrDefault();
                ventas.CLIENTES = (from c in db.CLIENTES where c.CLI_ID == CLI_CF_ID select c).FirstOrDefault();
            }
            else if (TC_ID == 2 && CLI_RI_ID != null)
            {
                ventas.CLI_ID = (from c in db.CLIENTES where c.CLI_ID == CLI_RI_ID select c.CLI_ID).FirstOrDefault();
                ventas.CLIENTES = (from c in db.CLIENTES where c.CLI_ID == CLI_RI_ID select c).FirstOrDefault();
            }
            else
            {
                //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                //ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_ID");
                //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_ID");

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

                return RedirectToAction("Create", "Ventas", vehSel);
            }

            //VALIDA QUE NO ESTÉ RESERVADO POR OTRO CLIENTE
            var reservado = (from r in db.RESERVAS
                             where r.VEH_ID == ventas.VEH_ID && r.RES_ESTADO == 41 && r.CLI_ID != ventas.CLI_ID
                             select r.VEH_ID).FirstOrDefault();

            if(reservado != null)
            {
                //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DESCRIPCIONES");
                ViewBag.EMPLEADO_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE");
                //ViewBag.FEC_ID_INICIO = new SelectList(db.FECHAS, "FEC_ID_INICIO", "FEC_ID");
                //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_PAT_MARCA_MODELO");
                ViewBag.SUCURSALES_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_ID");

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
                ModelState.AddModelError("CLI_ID", "EL VEHICULO ESTÁ RESERVADO POR OTRO CLIENTE");

                return RedirectToAction("Create", "Ventas", vehSel);
            }

            ventas.FECHAS = db.FECHAS.Find(ventas.FEC_ID);

            var FechaSel = from f in db.FECHAS
                           where f.FEC_FECHA == fecha
                              select f.FEC_ID;
            ventas.FEC_ID = FechaSel.FirstOrDefault();
            ventas.FECHAS = db.FECHAS.Find(ventas.FEC_ID);

            ventas.VEHICULOS = db.VEHICULOS.Find(ventas.VEH_ID);

            ventas.SUC_ID = SUCURSALES_ID;
            ventas.SUCURSALES = db.SUCURSALES.Find(ventas.SUC_ID);

            ventas.EMP_ID = EMPLEADO_ID;
            ventas.EMPLEADOS = db.EMPLEADOS.Find(ventas.EMP_ID);

            ventas.VEN_ESTADO = 51;
            ventas.VEN_VEH_MAR_ID_USADO = MAR_ID;

            if (ModelState.IsValid)
            {
                db.VENTAS.Add(ventas);
                db.SaveChanges();
                //return RedirectToAction("List");
            }


            try
            {
                return RedirectToAction("List", "Ventas");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // POST: /Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VENTAS ventas)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ventas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List","Ventas");
            }
            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DOC", ventas.CLI_ID);
            //ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_NOMBRE", ventas.EMP_ID);
            //ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_NOMBREDIA", ventas.FEC_ID);
            //ViewBag.VEH_ID = new SelectList(db.VEHICULOS, "VEH_ID", "VEH_MODELO", ventas.VEH_ID);
            return View(ventas);
        }

        // GET: /Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS ventas = db.VENTAS.Find(id);
            ventas.VEN_ESTADO = 52;

            if (ModelState.IsValid)
            {
                db.Entry(ventas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", "Ventas");
            }

          
            return View(ventas);
        }

        // POST: /Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VENTAS ventas = db.VENTAS.Find(id);
            db.VENTAS.Remove(ventas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         [HttpPost]
        public ActionResult Acciones(int ven_id, string Action)
        {
            try
            {

                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { ven_id = ven_id });
                }
                else if (Action == "Delete")
                {
                    return Delete(ven_id);
                }
                else if (Action == "Details")
                {
                    return RedirectToAction("Details", new { ven_id = ven_id });
                }
               
                else
                {
                    return RedirectToAction("List");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }}
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
