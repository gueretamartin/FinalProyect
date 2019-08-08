﻿using System;
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
            try 
            {
                VEHICULOS vehiculo = db.VEHICULOS.Find(veh_id);

                return View(vehiculo);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
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
                ViewBag.VEH_TIPOVEHICULO = new SelectList(db.TIPO_VEHICULOS, "TV_ID", "TV_DESCRIPCION");

                //MARCAS
                ViewBag.MAR_ID = new SelectList(db.MARCAS, "MAR_ID", "MAR_DESCRIPCION");

                //MARCAS
                ViewBag.VEH_TIPOCOMBUSTIBLE = new SelectList(db.TIPO_COMBUSTIBLES, "TCOM_ID", "TCOM_DESCRIPCION");

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
        // VALIDA Y CREA EL CLIENTE QUE DE LA VISTA DE CREAR CLIENTE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VEHICULOS vehiculos, Nullable<DateTime> fecha, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,
                            HttpPostedFileBase image4, string CLI_CUIT_ID, string CLI_CUIL_ID, string TC_ID)
        {
            try 
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
                    var veh = from v in db.VEHICULOS where v.ES_ID == 31 select v;

                    veh = veh.Where(v => v.VEH_PATENTE.Contains(vehiculos.VEH_PATENTE) 
                                            && v.VEH_VIGENTE.Equals(vehiculos.VEH_VIGENTE));

                    if (veh.Count() != 0)
                    {
                        ModelState.AddModelError("VEH_PATENTE", "PATENTE: La patente ya existe y está vigente.");
                        return this.Create();
                    }

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

                    if (String.IsNullOrEmpty(vehiculos.VEH_TIPOVEHICULO.ToString()))
                    {
                        ModelState.AddModelError("VEH_TIPOVEHICULO", "TIPO VEHÍCULO: Debe seleccionar un tipo de vehículo.");
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

                    if (String.IsNullOrEmpty(vehiculos.VEH_CILINDRADAS))
                    {
                        ModelState.AddModelError("VEH_CILINDRADAS", "CILINDRADA: Está vacío.");
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

                    if (String.IsNullOrEmpty(vehiculos.VEH_TIPOCOMBUSTIBLE.ToString()))
                    {
                        ModelState.AddModelError("VEH_TIPOCOMBUSTIBLE", "TIPO DE COMBUSTIBLE: Debe seleccionar un tipo de combustible.");
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

                vehiculos = ToUpperVehicle(vehiculos);

                if (ModelState.IsValid)
                {
                    db.VEHICULOS.Add(vehiculos);
                    db.SaveChanges();
                }

                if (image1 != null && image1.ContentLength > 0) { LoadImage(image1, vehiculos); }
                if (image2 != null && image2.ContentLength > 0) { LoadImage(image2, vehiculos); }
                if (image3 != null && image3.ContentLength > 0) { LoadImage(image3, vehiculos); }
                if (image4 != null && image4.ContentLength > 0) { LoadImage(image4, vehiculos); }
            
            
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }

        // TRANSFOMA TODOS LOS CAMPOS DEL CLIENTE A MAYÚSCULA
        private VEHICULOS ToUpperVehicle(VEHICULOS var_vehiculo)
        {
            try
            {
                if (!String.IsNullOrEmpty(var_vehiculo.VEH_MODELO))
                {
                    var_vehiculo.VEH_MODELO = var_vehiculo.VEH_MODELO.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_VERSION))
                {
                    var_vehiculo.VEH_VERSION = var_vehiculo.VEH_VERSION.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_CILINDRADAS))
                {
                    var_vehiculo.VEH_CILINDRADAS = var_vehiculo.VEH_CILINDRADAS.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_COLOR))
                {
                    var_vehiculo.VEH_COLOR = var_vehiculo.VEH_COLOR.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_KILOMETROS))
                {
                    var_vehiculo.VEH_KILOMETROS = var_vehiculo.VEH_KILOMETROS.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_DETALLES))
                {
                    var_vehiculo.VEH_DETALLES = var_vehiculo.VEH_DETALLES.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_PUERTAS))
                {
                    var_vehiculo.VEH_PUERTAS = var_vehiculo.VEH_PUERTAS.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_AÑO))
                {
                    var_vehiculo.VEH_AÑO = var_vehiculo.VEH_AÑO.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_PATENTE))
                {
                    var_vehiculo.VEH_PATENTE = var_vehiculo.VEH_PATENTE.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_STEREO_MODELO))
                {
                    var_vehiculo.VEH_STEREO_MODELO = var_vehiculo.VEH_STEREO_MODELO.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_vehiculo.VEH_STEREO_CODIGO))
                {
                    var_vehiculo.VEH_STEREO_CODIGO = var_vehiculo.VEH_STEREO_CODIGO.ToUpper();
                }

                return var_vehiculo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //CARGA LAS IMÁGENES
        public void LoadImage(HttpPostedFileBase imagen, VEHICULOS vehiculo)
        {
            IMAGENES img = new IMAGENES();
            img.VEH_ID = db.VEHICULOS.First(v => v.VEH_PATENTE.Equals(vehiculo.VEH_PATENTE) && v.VEH_VIGENTE == true && v.ES_ID == 31).VEH_ID;
            img.IMG_NAME = Path.GetFileName(imagen.FileName);
            img.IMG_IMAGE = ConvertToBytes(imagen);
            db.IMAGENES.Add(img);
            db.SaveChanges();
        }

        //CONVIERTE A BYTES
        public byte[] ConvertToBytes(HttpPostedFileBase imagen)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(imagen.InputStream);
            imageBytes = reader.ReadBytes((int)imagen.ContentLength);
            return imageBytes;
        }

        //TRAE LA PRIMER IMAGEN
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

        //Mostrar Imagen si existe alguna. 
        public ActionResult ShowImage(int veh_id)
        {
            var img = db.IMAGENES.Where(i=>i.VEH_ID == veh_id);
            if (img != null)
            {

            byte[] imagen = GetImageFromDataBase(veh_id);
            if (imagen != null)
            {
                return File(imagen, "image/jpg");
            }
            else return null;
           }
            else
            {
                return null;
            }
        }

        //TRAE TODAS LAS IMAGEN
        public ActionResult RetrieveImages(int veh_id, int img_id)
        {
            byte[] imagen = GetImageFromDataBase(veh_id, img_id);
            if (imagen != null)
            {
                return File(imagen, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        //TRAE TODAS LAS IMAGENES
        public List<int> BuscaImagenes(int veh_id)
        {
            List<int> imgList = db.IMAGENES.Where(x => x.VEH_ID == veh_id).Select(i => i.IMG_ID).ToList();
            return imgList;
        }


        //CONVIERTE LA PRIMER IMAGEN PARA MOSTRARLA
        public byte[] GetImageFromDataBase(int veh_id)
        {

             VEHICULOS veh = new VEHICULOS();
            veh = db.VEHICULOS.First(v => v.VEH_ID.Equals(veh_id) );
           
                IMAGENES image = db.IMAGENES.FirstOrDefault(i => i.VEH_ID == veh_id);
          
            if(image != null)
                {
                    byte[] imagenBytes = (from i in db.IMAGENES where i.IMG_ID == image.IMG_ID select i.IMG_IMAGE).First();

                    return imagenBytes;
                }
                else return null;
            
        }

        //CONVIERTE LAS IMAGENES PARA MOSTRARLAS
        public byte[] GetImageFromDataBase(int veh_id, int img_id)
        {
            byte[] imagenBytes = (from i in db.IMAGENES where i.IMG_ID == img_id select i.IMG_IMAGE).First();

            return imagenBytes;
        }
        

        // GET: /Vehiculos/Edit

        public ActionResult Edit(int veh_id)
        {
            try
            {
                VEHICULOS vehiculo = db.VEHICULOS.SingleOrDefault(v => v.VEH_ID == veh_id);

                if (vehiculo.ES_ID != 31)
                {
                    return RedirectToAction("Home", "Home");
                }

                ViewBag.MAR_ID = new SelectList(db.MARCAS, "MAR_ID", "MAR_DESCRIPCION", vehiculo.MAR_ID);

                ViewBag.EMP_ID = new SelectList(db.EMPLEADOS, "EMP_ID", "EMP_APELLIDO_NOMBRE", vehiculo.EMP_ID);

                ViewBag.FEC_ID = new SelectList(db.FECHAS, "FEC_ID", "FEC_FECHA", vehiculo.FEC_ID);

                ViewBag.SUC_ID = new SelectList(db.SUCURSALES, "SUC_ID", "SUC_DESCRIP", vehiculo.SUC_ID);

                ViewBag.VEH_TIPOVEHICULO = new SelectList(db.TIPO_VEHICULOS, "TV_ID", "TV_DESCRIPCION", vehiculo.VEH_TIPOVEHICULO);

                ViewBag.MON_ID = new SelectList(db.MONEDAS, "MON_ID", "MON_DESCRIPCION", vehiculo.MON_ID);

                ViewBag.VEH_TIPOCOMBUSTIBLE = new SelectList(db.TIPO_COMBUSTIBLES, "TCOM_ID", "TCOM_DESCRIPCION", vehiculo.VEH_TIPOCOMBUSTIBLE);

                return View(vehiculo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: /Vehiculos/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VEHICULOS vehiculos, DateTime fecha, string Metodo, string Patente, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,
                            HttpPostedFileBase image4, int img1, int img2, int img3, int img4)
        {
            vehiculos.CLIENTES = null;

            var FechaSel = from f in db.FECHAS
                           where f.FEC_FECHA == fecha
                           select f.FEC_ID;

            vehiculos.FEC_ID = FechaSel.FirstOrDefault();

            #region VALIDACIÓN DE CAMPOS DE VEHÍCULO
            if (!String.IsNullOrEmpty(vehiculos.SUC_ID.ToString()))
            {
                var veh = from v in db.VEHICULOS where v.ES_ID == 31 && v.VEH_PATENTE != Patente select v;

                veh = veh.Where(v => v.VEH_PATENTE.Contains(vehiculos.VEH_PATENTE)
                                        && v.VEH_VIGENTE.Equals(vehiculos.VEH_VIGENTE));

                if (veh.Count() != 0)
                {
                    ModelState.AddModelError("VEH_PATENTE", "PATENTE: La patente ya existe y está vigente.");
                    return this.Create();
                }

                if (String.IsNullOrEmpty(vehiculos.EMP_ID.ToString()))
                {
                    ModelState.AddModelError("EMP_ID", "EMPLEADO: Debe seleccionar un empleado.");
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

                if (String.IsNullOrEmpty(vehiculos.VEH_TIPOVEHICULO.ToString()))
                {
                    ModelState.AddModelError("VEH_TIPOVEHICULO", "TIPO VEHÍCULO: Debe seleccionar un tipo de vehículo.");
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

                if (String.IsNullOrEmpty(vehiculos.VEH_CILINDRADAS))
                {
                    ModelState.AddModelError("VEH_CILINDRADAS", "CILINDRADA: Está vacío.");
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

                if (String.IsNullOrEmpty(vehiculos.VEH_TIPOCOMBUSTIBLE.ToString()))
                {
                    ModelState.AddModelError("VEH_TIPOCOMBUSTIBLE", "TIPO DE COMBUSTIBLE: Debe seleccionar un tipo de combustible.");
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

            vehiculos = ToUpperVehicle(vehiculos);

            if (ModelState.IsValid)
            {
                db.Entry(vehiculos).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            if (image1 != null && image1.ContentLength > 0) { removeImage(img1); }
            if (image2 != null && image2.ContentLength > 0) { removeImage(img2); }
            if (image3 != null && image3.ContentLength > 0) { removeImage(img3); }
            if (image4 != null && image4.ContentLength > 0) { removeImage(img4); }

            if (image1 != null && image1.ContentLength > 0) { LoadImage(image1, vehiculos); }
            if (image2 != null && image2.ContentLength > 0) { LoadImage(image2, vehiculos); }
            if (image3 != null && image3.ContentLength > 0) { LoadImage(image3, vehiculos); }
            if (image4 != null && image4.ContentLength > 0) { LoadImage(image4, vehiculos); }

            return RedirectToAction("List");
        }

        private void removeImage(int img_id)
        {
            IMAGENES Image = db.IMAGENES.Find(img_id);
            db.IMAGENES.Remove(Image);
            db.SaveChanges();
        }

        // POST: /Vehiculos/Delete
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

        public ActionResult Sell(int veh_id)
        {
            VEHICULOS vehSelec = db.VEHICULOS.Include(v => v.SUCURSALES).Include(v => v.EMPLEADOS).Where(v => v.VEH_ID == veh_id).FirstOrDefault();

            //Para que sólo tome el vehículo y no sus dependencias
            //vehSelec.EMPLEADOS = null;
            //vehSelec.EMP_ID = 0;
            vehSelec.CLI_ID = 0;
            //vehSelec.SUC_ID = 0;


            //return RedirectToAction("Create", "Reservas", vehSelec);
            return RedirectToAction("Create", "Ventas", vehSelec);

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
                else if (Action == "Sell")
                {
                    return RedirectToAction("Sell", new { veh_id = veh_id });
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

        public ActionResult UpdateState(int veh_id, int ESTADO_ID, string operation)
        {

            VEHICULOS vehSelect = db.VEHICULOS.Include(v => v.CLIENTES).
                                            Include(v => v.EMPLEADOS).
                                            Include(v => v.FECHAS).
                                            Include(v => v.SUCURSALES).
                                            Include(v => v.IMAGENES).
                                            SingleOrDefault(x => x.VEH_ID == veh_id);

            vehSelect.ES_ID = ESTADO_ID;
            vehSelect.ESTADOS = db.ESTADOS.Find(ESTADO_ID);

            if (ModelState.IsValid)
            {
                db.Entry(vehSelect).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (operation.Equals("Reserve"))
            {
                return RedirectToAction("ListActivas", "Reservas");
            }
            else if (operation.Equals("Sell"))
            {
                return RedirectToAction("List", "Ventas");
            }
            else
            {
                return RedirectToAction("List", "Vehiculos");
            }

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
