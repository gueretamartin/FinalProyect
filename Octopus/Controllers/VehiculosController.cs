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

        // GET: /Vehiculos/

        // GET: /Vehiculos/Details/5
        public ActionResult Details(int veh_id)
        {
            VEHICULOS vehiculo = db.VEHICULOS.Include(v => v.CLIENTES).
                                            Include(v => v.EMPLEADOS).
                                            Include(v => v.FECHAS).
                                            Include(v => v.SUCURSALES).
                                            Include(v => v.IMAGENES).
                                            SingleOrDefault(x => x.VEH_ID == veh_id);
            return View(vehiculo);
        }

        // GET: /Vehiculos/Create
        public ActionResult Create()
        {
            //ViewBag.CLI_ID = new SelectList(db.CLIENTES, "CLI_ID", "CLI_ID");
            ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE");
            ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA");
            ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP");

            //CONDICION IVA
            var condiciones = (from c in db.CLIENTES
                               select c.TC_ID).Distinct();
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

            return View();

        }

        // POST: /Vehiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VEHICULOS vehiculos, DateTime fecha, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, 
                            HttpPostedFileBase image4, string CLI_RI_ID, string CLI_CF_ID, string TC_ID)
        {
            // VALIDA TIPO DE CLIENTE Y DNI/CUIT
            if (TC_ID.Equals("CONSUMIDOR_FINAL") && !String.IsNullOrEmpty(CLI_CF_ID))
            {
                vehiculos.CLI_ID = (from c in db.CLIENTES where c.CLI_DOC == CLI_CF_ID select c.CLI_ID).FirstOrDefault();
            }
            else if(TC_ID.Equals("RESPONSABLE_INSCRIPTO") && !String.IsNullOrEmpty(CLI_RI_ID))
            {
                vehiculos.CLI_ID = (from c in db.CLIENTES where c.CLI_RI_CUIT == CLI_RI_ID select c.CLI_ID).FirstOrDefault();
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
            img.VEH_ID = db.VEHICULOS.First(v => v.VEH_PATENTE.Equals(vehiculo.VEH_PATENTE) && v.VEH_VIGENTE == true).VEH_ID;
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

        public ActionResult List(string searchVehicle)
        {
            var vehiculos = db.VEHICULOS.Include(v => v.CLIENTES).Include(v => v.EMPLEADOS).Include(v => v.FECHAS).Include(v => v.SUCURSALES);
            if (!String.IsNullOrEmpty(searchVehicle))
            {
                vehiculos = vehiculos.Where(v => v.VEH_PATENTE.Contains(searchVehicle)
                    || v.VEH_MARCA.Contains(searchVehicle)
                    || v.VEH_MODELO.Contains(searchVehicle));
            }
  

            return View(vehiculos.ToList());
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

        // POST: /Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int veh_id)
        {
            VEHICULOS vehiculo = db.VEHICULOS.Find(veh_id);
            db.VEHICULOS.Remove(vehiculo);
            db.SaveChanges();
            return RedirectToAction("List");
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
                else
                {
                    return RedirectToAction("List");
                }
            }
            catch(Exception ex)
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
