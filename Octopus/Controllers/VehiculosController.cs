using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using System.IO;

namespace Octopus.Controllers
{
    public class VehiculosController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        // GET: /Vehiculos/List?searchVehicle=[Parámetro]
        // LEVANTA LA VISTA DE LISTADO DE VEHICULOS O LISTA DE VEHICULOS QUE CONTIENEN CON EL PARÁMETRO PASADO
        public ActionResult List(string searchVehicle)
        {
            try
            { 
                ViewBag.VW_MAR = new SelectList(db.VW_MARCAS, "VW_MAR_ID", "VW_MAR_DESCRIPCION");

                ViewBag.VW_TV = new SelectList(db.TIPO_VEHICULOS, "TV_ID", "TV_DESCRIPCION");

                var vehiculos = from v in db.VEHICULOS where v.ES_ID == 31 && v.VEH_VIGENTE == true select v;
            
                if (!String.IsNullOrEmpty(searchVehicle))
                {
                    vehiculos = vehiculos.Where(v => v.MARCAS.MAR_DESCRIPCION.Contains(searchVehicle)
                        || v.VEH_MODELO.Contains(searchVehicle)
                        || v.VEH_AÑO.Contains(searchVehicle)
                        || v.VEH_VERSION.Contains(searchVehicle)
                        || v.VEH_PATENTE.Contains(searchVehicle)
                        || v.TIPO_COMBUSTIBLES.TCOM_DESCRIPCION.Contains(searchVehicle)
                        || v.SUCURSALES.SUC_DESCRIP.Contains(searchVehicle)
                        || v.TIPO_VEHICULOS.TV_DESCRIPCION.Contains(searchVehicle)
                    );
                }


                return View(vehiculos.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }

        // GET: /Vehiculos/

        // GET: /Vehiculos/Details/5
        
        public ActionResult Details(int veh_id)
        {
            VEHICULOS vehiculo = db.VEHICULOS.Find(veh_id);

            return View(vehiculo);
        }
        
          
        // GET: /Vehiculos/Create
        
        public ActionResult Create()
        {
            try 
            { 
                //LISTA DE SUCURSALES
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");

                //LISTA DE EMPLEADOS
                ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE");

                //TIPO CLIENTE
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                //FECHAS
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA");

                //CLIENTES CUIT
                var CLI_CUIT = (from c in db.CLIENTES
                                where c.TC_ID == 2 && c.ES_ID == 1
                                select c
                                );

                ViewBag.CLI_CUIT_ID = new SelectList(CLI_CUIT, "CLI_ID", "CLI_RAZONSOCIAL_CUIT");

                //CLIENTES CUIL
                var CLI_CUIL = (from c in db.CLIENTES
                            where c.TC_ID == 1 && c.ES_ID == 1
                            select c
                            );

                ViewBag.CLI_CUIL_ID = new SelectList(CLI_CUIL, "CLI_ID", "CLI_APELLIDO_NOMBRE_CUIL");

                //TIPO DE VEHÍCULO
                ViewBag.TV_ID = new SelectList(db.TIPO_VEHICULOS, "TV_ID", "TV_DESCRIPCION");

                //MARCAS
                ViewBag.MAR_ID = new SelectList(db.MARCAS, "MAR_ID", "MAR_DESCRIPCION");

                //MARCAS
                ViewBag.TCOM_ID = new SelectList(db.TIPO_COMBUSTIBLES, "TCOM_ID", "TCOM_DESCRIPCION");

                //MARCAS
                ViewBag.MON_ID = new SelectList(db.MONEDAS, "MON_ID", "MON_DESCRIPCION");

                return View();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }

        }

        // POST: /Vehiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VEHICULOS vehiculos, Nullable<DateTime> fecha, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,
                            HttpPostedFileBase image4, string CLI_CUIT_ID, string CLI_CUIL_ID, string TC_ID)
        {
            if (!String.IsNullOrEmpty(CLI_CUIT_ID))
            {
                int CLI_ID_INT = int.Parse(CLI_CUIT_ID);
                var ClienteSel = from c in db.CLIENTES where c.CLI_ID == CLI_ID_INT select c.CLI_ID;
                vehiculos.CLI_ID = ClienteSel.FirstOrDefault();
            }
            else if (!String.IsNullOrEmpty(CLI_CUIL_ID))
            {
                int CLI_ID_INT = int.Parse(CLI_CUIL_ID);
                var ClienteSel = from c in db.CLIENTES where c.CLI_ID == CLI_ID_INT select c.CLI_ID;
                vehiculos.CLI_ID = ClienteSel.FirstOrDefault();
            }

            if(!fecha.Equals(""))
            {
                var FechaSel = from f in db.FECHAS where f.FEC_FECHA == fecha select f.FEC_ID;
                vehiculos.FEC_ID = FechaSel.FirstOrDefault();
            }

            vehiculos.VEH_VIGENTE = true;

            vehiculos.ES_ID = 31;
            
            #region VALIDACIÓN DE CAMPOS DE VEHÍCULO
            if (!String.IsNullOrEmpty(vehiculos.SUC_ID.ToString()))
            {
                if (String.IsNullOrEmpty(vehiculos.EMP_ID.ToString()))
                {
                    ModelState.AddModelError("EMP_ID", "EMPLEADO: Debe seleccionar un empleado.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(TC_ID))
                {
                    ModelState.AddModelError("CLI_ID", "TIPO DE CLIENTE: Debe seleccionar un tipo de cliente.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.CLI_ID.ToString()))
                {
                    ModelState.AddModelError("CLI_ID", "CLIENTE: Debe seleccionar un cliente.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.FEC_ID.ToString()))
                {
                    ModelState.AddModelError("FEC_ID", "FECHA: Debe seleccionar una fecha.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.TV_ID.ToString()))
                {
                    ModelState.AddModelError("TV_ID", "TIPO VEHÍCULO: Debe seleccionar un tipo de vehículo.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.MAR_ID.ToString()))
                {
                    ModelState.AddModelError("MAR_ID", "MARCA: Debe seleccionar una marca.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_MODELO))
                {
                    ModelState.AddModelError("VEH_MODELO", "MODELO: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_VERSION))
                {
                    ModelState.AddModelError("VEH_VERSION", "VERSIÓN: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_PATENTE))
                {
                    ModelState.AddModelError("VEH_PATENTE", "PATENTE: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_AÑO))
                {
                    ModelState.AddModelError("VEH_AÑO", "AÑO: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_CILINDRADA))
                {
                    ModelState.AddModelError("VEH_CILINDRADA", "CILINDRADA: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_KILOMETROS))
                {
                    ModelState.AddModelError("VEH_KILOMETROS", "KILÓMETROS: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_COLOR))
                {
                    ModelState.AddModelError("VEH_COLOR", "COLOR: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_PUERTAS))
                {
                    ModelState.AddModelError("VEH_PUERTAS", "PUERTAS: Está vacío.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.TCOM_ID.ToString()))
                {
                    ModelState.AddModelError("TCOM_ID", "TIPO DE COMBUSTIBLE: Debe seleccionar un tipo de combustible.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.MON_ID.ToString()))
                {
                    ModelState.AddModelError("MON_ID", "MONEDA: Debe seleccionar una moneda.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.VEH_PRECIO_INGRESO.ToString()))
                {
                    ModelState.AddModelError("VEH_PRECIO_INGRESO", "PRECIO DE INGRESO: Está vacío.");
                    return this.Create();
                }
            }
            else
            {
                ModelState.AddModelError("SUC_ID", "SUCURSAL: Debe seleccionar una sucursal.");
                return this.Create();
            }

            #endregion


            /*
            if (!String.IsNullOrEmpty(TC_ID) && (!String.IsNullOrEmpty(CLI_CUIT_ID) || !String.IsNullOrEmpty(CLI_CUIL_ID)))
            {
                int TC_ID_int = int.Parse(TC_ID);
                int CLI_CUIT_ID_int = int.Parse(CLI_CUIT_ID);
                int CLI_CUIL_ID_int = int.Parse(CLI_CUIL_ID);

                vehiculos.CLI_ID = (from c in db.CLIENTES
                                    where (c.CLI_ID == CLI_CUIT_ID_int || c.CLI_ID == CLI_CUIL_ID_int)
                                    && c.TC_ID == TC_ID_int
                                    select c.CLI_ID).FirstOrDefault();
            }
            else
            {
                ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE");
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA");
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");

                //CONDICION IVA
                var condiciones = (from c in db.CLIENTES
                                   select c.TC_ID
                                       ).Distinct();
                ViewBag.TC_ID = new SelectList(condiciones);

                //CLIENTES EMPRESAS
                var empresas = (from c in db.CLIENTES
                                where c.TC_ID == 2
                                select c.CLI_RI_CUIT).Distinct();
                ViewBag.CLI_RI_ID = new SelectList(empresas);

                //CLIENTES FINALES
                var consumidores = (from c in db.CLIENTES
                                    where c.TC_ID == 1
                                    select c.CLI_DOC).Distinct();
                ViewBag.CLI_CF_ID = new SelectList(consumidores);
                ModelState.AddModelError("CLI_ID", "LOS CAMPOS 'CONDICION IVA' Y 'DNI/CUIT' SON OBLIGATORIOS");
                return View("Create");
            }

            
            var FechaSel = from f in db.FECHAS
                           where f.FEC_FECHA == fecha
                            select f.FEC_ID;
            vehiculos.FEC_ID = FechaSel.FirstOrDefault();
            vehiculos.VEH_VIGENTE = true;
            if (ModelState.IsValid)
            {
                db.VEHICULOS.Add(vehiculos);
                db.SaveChanges();
            }
            */

            if (ModelState.IsValid)
            {
                db.VEHICULOS.Add(vehiculos);
                db.SaveChanges();
            }

            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DNI");
            //ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE");
            //ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA");
            //ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");

            if (image1 != null && image1.ContentLength > 0) { LoadImage(image1, vehiculos); }
            if (image2 != null && image1.ContentLength > 0) { LoadImage(image2, vehiculos); }
            if (image3 != null && image1.ContentLength > 0) { LoadImage(image3, vehiculos); }
            if (image4 != null && image1.ContentLength > 0) { LoadImage(image4, vehiculos); }
            
            
            return RedirectToAction("List");
        }
        
        
        public void LoadImage(HttpPostedFileBase imagen, VEHICULOS vehiculo)
        {
            IMAGENES img = new IMAGENES();
            img.VEH_ID = db.VEHICULOS.First(v => v.VEH_PATENTE.Equals(vehiculo.VEH_PATENTE) && v.VEH_VIGENTE == true && v.ES_ID == 31).VEH_ID;
            img.IMG_NAME = Path.GetFileName(imagen.FileName);
            img.IMG_IMAGE = ConvertToBytes(imagen);
            db.IMAGENES.Add(img);
            db.SaveChanges();
        }

        public byte[] ConvertToBytes(HttpPostedFileBase imagen)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(imagen.InputStream);
            imageBytes = reader.ReadBytes((int)imagen.ContentLength);
            return imageBytes;
        }

        //Trae la primer imagen
        public ActionResult RetrieveImage(int veh_id)
        {
            byte[] imagen = GetImageFromDataBase(veh_id);
            if (imagen != null)
            {
                return File(imagen, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        //Trae todas las imagenes
        public List<int> BuscaImagenes(int veh_id)
        {
            List<int> imgList = db.IMAGENES.Where(x => x.VEH_ID == veh_id).Select(i => i.IMG_ID).ToList();
            return imgList;
        }


        //Convierte la imagen para mostrarla
        public byte[] GetImageFromDataBase(int veh_id)
        {
            int imageID = db.IMAGENES.Where(i => i.VEH_ID == veh_id).Max(i => i.IMG_ID);
            byte[] imagenBytes = (from i in db.IMAGENES where i.IMG_ID == imageID select i.IMG_IMAGE).First();

            return imagenBytes;

            //var imagenDB = from img in db.IMAGENES where img.VEH_ID == veh_id select img.IMG_IMAGE;
            //byte[] imagenBytes = imagenDB.First();
        }

        public byte[] GetImageFromDataBase(int veh_id, int img_id)
        {
            int imageID = db.IMAGENES.Where(i => i.VEH_ID == veh_id).Max(i => i.IMG_ID);
            byte[] imagenBytes = (from i in db.IMAGENES where i.IMG_ID == imageID select i.IMG_IMAGE).First();

            return imagenBytes;

            //var imagenDB = from img in db.IMAGENES where img.VEH_ID == veh_id select img.IMG_IMAGE;
            //byte[] imagenBytes = imagenDB.First();
        }


        // GET: /Vehiculos/Edit/5
        /*
        public ActionResult Edit(int veh_id)
        {
            try
            {
                VEHICULOS vehiculo = db.VEHICULOS.Include(v => v.CLIENTES).
                                                Include(v => v.EMPLEADOS).
                                                Include(v => v.FECHAS).
                                                Include(v => v.SUCURSALES).
                                                Include(v => v.IMAGENES).
                                                SingleOrDefault(x => x.VEH_ID == veh_id);

                ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_DNI_APELLIDO_NOMBRE", vehiculo.CLI_ID);
                ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE", vehiculo.EMP_ID);
                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA", vehiculo.FEC_ID);
                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", vehiculo.SUC_ID);
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: /Vehiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VEHICULOS vehiculos, DateTime fecha, string Metodo)
        {
            var FechaSel = from f in db.FECHAS
                           where f.FEC_FECHA == fecha
                           select f.FEC_ID;
            vehiculos.FEC_ID = FechaSel.FirstOrDefault();
            FECHAS FecActual = db.FECHAS.Find(vehiculos.FEC_ID);
            vehiculos.FECHAS = FecActual;

            SUCURSALES SucActual = db.SUCURSALES.Find(vehiculos.SUC_ID);
            vehiculos.SUCURSALES = SucActual;

            CLIENTES CliActual = db.CLIENTES.Find(vehiculos.CLI_ID);
            vehiculos.CLIENTES = CliActual;

            EMPLEADOS EmpActual = db.EMPLEADOS.Find(vehiculos.EMP_ID);
            vehiculos.EMPLEADOS = EmpActual;

            if (ModelState.IsValid)
            {
                db.Entry(vehiculos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }
        */

        // POST: /Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult Delete(int veh_id)
        {
            try
            {
                VEHICULOS vehiculos = db.VEHICULOS.Find(veh_id);

                if (vehiculos == null)
                {
                    return RedirectToAction("Home", "Home");
                }


                if (vehiculos.ES_ID != 31)
                {
                    return RedirectToAction("Home", "Home");
                }

                vehiculos.ES_ID = 32;

                if (ModelState.IsValid)
                {
                    db.Entry(vehiculos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }

        public ActionResult Reserve(int veh_id)
        {
            VEHICULOS vehSelec = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == veh_id).FirstOrDefault();

            //Para que sólo tome el vehículo y no sus dependencias
            //vehSelec.EMPLEADOS = null;
            //vehSelec.EMP_ID = 0;
            vehSelec.CLI_ID = 0;
            //vehSelec.SUC_ID = 0;


            //return RedirectToAction("Create", "Reservas", vehSelec);
            return RedirectToAction("Create", "Reservas", vehSelec);

            //Controller cont = new ReservasController();
            //cont.create
        }

        [HttpPost]
        public ActionResult Acciones(int veh_id, string Action)
        {
            try
            {

                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { veh_id = veh_id });
                }
                else if (Action == "Delete")
                {
                    return Delete(veh_id);
                }
                else if (Action == "Details")
                {
                    return RedirectToAction("Details", new { veh_id = veh_id });
                }
                else if (Action == "Reserve")
                {
                    return RedirectToAction("Reserve", new { veh_id = veh_id });
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
