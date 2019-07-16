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

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spire.Pdf;
using System.Drawing;
using Spire.Pdf.Widget;
using Spire.Pdf.Fields;
using System.Threading;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.Graphics;
using System.Data.Entity.Validation;


namespace Octopus.Controllers
{
    public class PresupuestosController : Controller
    {

        private OctopusEntities db = new OctopusEntities();

        
        // GET: /Presupuestos/
         public ActionResult OpenPDF(string filename)
        {
            PdfDocument doc = new PdfDocument();
          

            System.Diagnostics.Process.Start(Server.MapPath("~/RPTS/") + filename);


            


             return RedirectToAction("Report");
        }

        
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

            int cont = 0;
            if (presupuestos.img1path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img2path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img3path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img4path != null)
            {
                cont = cont + 1;
            }

            presupuestos.Contador = cont;
            return View(presupuestos);
        }

        public ActionResult Details_Report(int? id)
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

            int cont = 0;
            if (presupuestos.img1path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img2path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img3path != null)
            {
                cont = cont + 1;
            }
            if (presupuestos.img4path != null)
            {
                cont = cont + 1;
            }

            presupuestos.Contador = cont;
            return View(presupuestos);
        }

        public ActionResult Create()
        {




            var empleados = db.EMPLEADOS.Where(c => c.EMP_ESTADO == 1);
            var clientes = db.CLIENTES.Where(c => c.ES_ID == 1);
            var marcas = db.MARCAS.Where(c => c.MAR_ID != null);
            //    //// var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            //    //ViewBag.Rol = new SelectList(db.PRESUPUESTOS_TIPOS, "ROL_ID", "ROL_DESC");
            //    ////ViewBag.Estado = new SelectList(states, "ES_ID", "ES_DESCRIPCION");
            ViewBag.Marcas = new SelectList(marcas, "MAR_ID", "MAR_DESCRIPCION");
            ViewBag.Empleados = new SelectList(empleados, "EMP_ID", "EMP_APELLIDO_NOMBRE");
            ViewBag.Clientes = new SelectList(clientes, "CLI_ID", "CLI_APELLIDO_NOMBRE");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PRESUPUESTOS var_presupuesto, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,
                            HttpPostedFileBase image4, DateTime fechaPresFin)
        {
            
            int fecha_hoy = int.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));
            int fecha_hasta = int.Parse(string.Format("{0:yyyyMMdd}", fechaPresFin));
            if (fecha_hoy > fecha_hasta)
            {
                ModelState.AddModelError("PRE_FECHA_FIN", "Fecha Hasta: La fecha de validez debe superar la fecha actual.");
                return this.Create();
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
                //        db.PRESUPUESTOS.Add(var_presupuesto);
                //      db.SaveChanges();

                if (image1 != null)
                {
                    String path1 = LoadImage(image1, var_presupuesto);
                    var_presupuesto.img1path = path1;
                }
                if (image2 != null)
                {
                    String path2 = LoadImage(image2, var_presupuesto);
                    var_presupuesto.img2path = path2;
                }
                if (image3 != null)
                {
                    String path3 = LoadImage(image3, var_presupuesto);
                    var_presupuesto.img3path = path3;
                }
                if (image4 != null)
                {
                    String path4 = LoadImage(image4, var_presupuesto);
                    var_presupuesto.img4path = path4;
                }

                db.PRESUPUESTOS.Add(var_presupuesto);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Home", "Home");
            }
          
        }


        public String LoadImage(HttpPostedFileBase imageModel, PRESUPUESTOS presupuesto)
        {

            string fileName = Path.GetFileNameWithoutExtension(imageModel.FileName);
            string extension = Path.GetExtension(imageModel.FileName);
            fileName = fileName + "_" + DateTime.Now.ToString("yyyymmssfff") + extension;
            // imageModel.ImagePath = "~/Image/" + fileName;
            string strFinalPath = string.Empty;
            string normalizedFirstPath = Server.MapPath("~/Image/").TrimEnd(new char[] { '\\' });
            string normalizedSecondPath = fileName.TrimStart(new char[] { '\\' });
            strFinalPath = Path.Combine(normalizedFirstPath, normalizedSecondPath);
            strFinalPath.Replace("\\\\", "\\");
            imageModel.SaveAs(Server.MapPath("~/Image/") + fileName);
            return fileName;
         
        }


  


        // GET: /Presupuestos/
        public ActionResult Report(string searchPresupuesto, int? page)
        {

            try
            {
                return View(Directory.EnumerateFiles(Server.MapPath("~/RPTS/")).ToPagedList(page ?? 1, 6));
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }

        }


        public ActionResult List(string searchPresupuesto, int? page)
        {

            try
            {

                

                var presupuestos = from c in db.PRESUPUESTOS select c;
                presupuestos = presupuestos.Where(c => c.ES_ID == 1);
                if (!String.IsNullOrEmpty(searchPresupuesto))
                {
                 presupuestos =    presupuestos.Where(c => 
                        c.MARCAS.MAR_DESCRIPCION.Contains(searchPresupuesto)
                       || c.CLIENTES.CLI_NOMBRE.Contains(searchPresupuesto)
                       || c.CLIENTES.CLI_APELLIDO.Contains(searchPresupuesto)
                        || c.PRE_MODELO.Contains(searchPresupuesto)
                        || c.PRE_ANIO.Contains(searchPresupuesto)
                        || c.PRE_VERSION.Contains(searchPresupuesto)
                        || c.PRE_PATENTE.Contains(searchPresupuesto)
                        );

                }
                return View(presupuestos.ToList().ToPagedList(page ?? 1, 6));
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
            else if (Action == "Print")
            {
                return RedirectToAction("Print", new { pre_id = pre_id });
            }
           
            else
            {
                return View("List");
            }
        }


        static PdfPageTemplateElement CreateHeaderTemplate(PdfDocument doc, PdfMargins margins, String imagenray, int? id)
        {
            //get page size
            SizeF pageSize = doc.PageSettings.Size;

            //create a PdfPageTemplateElement object as header space
            PdfPageTemplateElement headerSpace = new PdfPageTemplateElement(pageSize.Width, margins.Top);
            headerSpace.Foreground = false;

            //declare two float variables
            float x = margins.Left;
            float y = 0;
            
            //draw image in header space 
            PdfImage headerImage = PdfImage.FromFile(imagenray);
            float width = headerImage.Width / 3;
            float height = headerImage.Height / 3;
            headerSpace.Graphics.DrawImage(headerImage, x, margins.Top - height - 2, width, height);

            //draw line in header space
            PdfPen pen = new PdfPen(PdfBrushes.Gray, 1);
            headerSpace.Graphics.DrawLine(pen, x, y + margins.Top - 2, pageSize.Width - x, y + margins.Top - 2);

            //draw text in header space
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Impact", 25f, FontStyle.Bold));
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Left);
            String headerText = "PRESUPUESTO N°" + id;
            SizeF size = font.MeasureString(headerText, format);
            headerSpace.Graphics.DrawString(headerText, font, PdfBrushes.Gray, pageSize.Width - x - size.Width - 2, margins.Top - (size.Height + 5), format);

            //return headerSpace
            return headerSpace;
        }


        public ActionResult Print(int? pre_id)
        {
            var pres = new PRESUPUESTOS();
            pres = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == pre_id);


            //Create a pdf document.
            PdfDocument doc = new PdfDocument();
            // Create one page

            doc.PageSettings.Size = PdfPageSize.A4;

            //reset the default margins to 0
            doc.PageSettings.Margins = new PdfMargins(0);

            //create a PdfMargins object, the parameters indicate the page margins you want to set
            PdfMargins margins = new PdfMargins(60, 60, 60, 60);
           
            String imgagenray = Server.MapPath("~/Image/") + "rayco.jpg";
            //create a header template with content and apply it to page template
            doc.Template.Top = CreateHeaderTemplate(doc, margins, imgagenray, pre_id);

            //apply blank templates to other parts of page template
            doc.Template.Bottom = new PdfPageTemplateElement(doc.PageSettings.Size.Width, margins.Bottom);
            doc.Template.Left = new PdfPageTemplateElement(margins.Left, doc.PageSettings.Size.Height);
            doc.Template.Right = new PdfPageTemplateElement(margins.Right, doc.PageSettings.Size.Height);
            
            
            PdfPageBase page = doc.Pages.Add();

            PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            PdfStringFormat centerAligment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
         


            page.Canvas.DrawString("Fecha: " + string.Format("{0:yyyy-MM-dd}", DateTime.Now),

                                   new PdfFont(PdfFontFamily.Courier, 10f),

                                   new PdfSolidBrush(Color.Black),

                                   10, 70);


            page.Canvas.DrawString("Atendido por: " + pres.EMPLEADOS.EMP_APELLIDO_NOMBRE,

                               new PdfFont(PdfFontFamily.Courier, 10f),

                               new PdfSolidBrush(Color.Black),

                               10, 90);

            page.Canvas.DrawString("Cliente: " + pres.CLIENTES.CLI_APELLIDO_NOMBRE,

                              new PdfFont(PdfFontFamily.Courier, 10f),

                              new PdfSolidBrush(Color.Black),

                              10, 110);


            // proximo a 70 
            page.Canvas.DrawString("Fecha de Vencimiento: " + pres.FechaHastaEdicion,

                                   new PdfFont(PdfFontFamily.Courier, 15f, PdfFontStyle.Bold),

                                   new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width,
                                   
                                   20, rightAlignment);

            page.Canvas.DrawString("DATOS DEL VEHÍCULO:",

                                   new PdfFont(PdfFontFamily.Courier, 15f, PdfFontStyle.Bold),

                                   new PdfSolidBrush(Color.Black),

                                   20, 145);

            page.Canvas.DrawString(" -  Año: " + pres.PRE_ANIO,

                                   new PdfFont(PdfFontFamily.Courier, 10f),

                                   new PdfSolidBrush(Color.Black),

                                   25, 180);

            page.Canvas.DrawString(" -  Marca: " + pres.MARCAS.MAR_DESCRIPCION,

                                   new PdfFont(PdfFontFamily.Courier, 10f),

                                   new PdfSolidBrush(Color.Black),

                                   25, 200);

            page.Canvas.DrawString(" -  Modelo: " + pres.PRE_MODELO,

                                new PdfFont(PdfFontFamily.Courier, 10f),

                                new PdfSolidBrush(Color.Black),

                                25, 220);

            page.Canvas.DrawString(" -  Versión: " + pres.PRE_VERSION,

                               new PdfFont(PdfFontFamily.Courier, 10f),

                               new PdfSolidBrush(Color.Black),

                               25, 240);


            page.Canvas.DrawString(" -  Patente: " + pres.PRE_PATENTE,

                               new PdfFont(PdfFontFamily.Courier, 10f),

                               new PdfSolidBrush(Color.Black),

                               25, 260);

            page.Canvas.DrawString("OTROS DETALLES",

                                   new PdfFont(PdfFontFamily.Courier, 15f, PdfFontStyle.Bold),

                                   new PdfSolidBrush(Color.Black),

                                   20, 295);

            String chasis = "Sin Detalle";
            if (pres.PRE_ESTADO_CHASIS != null)
            {
                chasis = pres.PRE_ESTADO_CHASIS;
            }


            page.Canvas.DrawString(" -  Estado Chasis: " + chasis
            ,

                              new PdfFont(PdfFontFamily.Courier, 10f),

                              new PdfSolidBrush(Color.Black),

                              25, 330);

            String pintura = "Sin Detalle";
            if (pres.PRE_ESTADO_PINTURA != null)
            {
                pintura = pres.PRE_ESTADO_PINTURA;
            }

            page.Canvas.DrawString(" -  Estado Pintura: " + pintura
          ,

                            new PdfFont(PdfFontFamily.Courier, 10f),

                            new PdfSolidBrush(Color.Black),

                            25, 350);

            String parab = "Sin Detalle";
            if (pres.PRE_ESTADO_PARABRISAS != null)
            {
                parab = pres.PRE_ESTADO_PARABRISAS;
            }

            page.Canvas.DrawString(" -  Estado Parabrisas: " + parab
          ,

                            new PdfFont(PdfFontFamily.Courier, 10f),

                            new PdfSolidBrush(Color.Black),

                            25, 370);

            String neu = "Sin Detalle";
            if (pres.PRE_ESTADO_NEUMATICOS != null)
            {
                neu = pres.PRE_ESTADO_NEUMATICOS;
            }

            page.Canvas.DrawString(" -  Estado Neumáticos: " + neu
          ,

                            new PdfFont(PdfFontFamily.Courier, 10f),

                            new PdfSolidBrush(Color.Black),

                            25, 390);

            String inter = "Sin Detalle";
            if (pres.PRE_ESTADO_INTERIOR != null)
            {
                inter = pres.PRE_ESTADO_INTERIOR;
            }

            page.Canvas.DrawString(" -  Estado Interior: " + inter
          ,

                            new PdfFont(PdfFontFamily.Courier, 10f),

                            new PdfSolidBrush(Color.Black),

                            25, 410);

            String otro = "Sin Detalle";

            if (pres.PRE_DETALLES != null)
            {
                otro = pres.PRE_DETALLES;
            }

            page.Canvas.DrawString(" -  Otros detalles: " + otro
          ,

                            new PdfFont(PdfFontFamily.Courier, 10f),

                            new PdfSolidBrush(Color.Black),

                            25, 430);

            page.Canvas.DrawString("Total: $" + pres.PRE_PRECIO
          ,

                            new PdfFont(PdfFontFamily.Courier, 20f),
                            new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width / 2 ,

                                  500, centerAligment);
                           

            page.Canvas.DrawString("_______________________________"
        ,

                          new PdfFont(PdfFontFamily.Courier, 5f),

                          new PdfSolidBrush(Color.Black),

                          20, 700);
            page.Canvas.DrawString("FIRMA DEL EMPLEADO"
          ,

                            new PdfFont(PdfFontFamily.Courier, 5f),

                            new PdfSolidBrush(Color.Black),

                            20, 715);

            page.Canvas.DrawString("_______________________________",

                                   new PdfFont(PdfFontFamily.Courier, 5f),

                                   new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width,

                                   700, rightAlignment);

            page.Canvas.DrawString("FIRMA DEL CLIENTE" ,

                                   new PdfFont(PdfFontFamily.Courier, 5f),

                                   new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width,

                                   715, rightAlignment);


            //Save pdf file.
            String name = Server.MapPath("~/RPTS/") + string.Format("{0:yyyyMMddHHss}", DateTime.Now) + "_" + pres.CLIENTES.CLI_APELLIDO_NOMBRE + ".pdf";

            doc.SaveToFile(name);

            doc.Close();



            //Launching the Pdf file.



            return RedirectToAction("Report", "Presupuestos");
        }


        public ActionResult Edit(int? pre_id)
        {

            var model = new PRESUPUESTOS();
            model = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == pre_id);
            var empleados = db.EMPLEADOS.Where(c => c.EMP_ESTADO == 1);
            var states = db.ESTADOS.Where(c => c.ES_ID == 2 || c.ES_ID == 1);
            var clientes = db.CLIENTES.Where(c => c.ES_ID == 1);
            var marcas = db.MARCAS;

            model.Marcas_List = new SelectList(marcas, "MAR_ID", "MAR_DESCRIPCION", model.PRE_MARCA);
            model.Estados_List = new SelectList(states, "ES_ID", "ES_DESCRIPCION", model.ES_ID);
            model.Empleados_List = new SelectList(empleados, "EMP_ID", "EMP_APELLIDO_NOMBRE", model.EMP_ID);
            model.Clientes_List = new SelectList(clientes, "CLI_ID", "CLI_APELLIDO_NOMBRE", model.EMP_ID);
            
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PRESUPUESTOS var_presupuesto, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,
                            HttpPostedFileBase image4, DateTime fechaPresFin)
        {


            //  var_presupuesto.img1path = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == var_presupuesto.PRE_ID).img1path;
            //  var_presupuesto.img2path = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == var_presupuesto.PRE_ID).img2path;
            //  var_presupuesto.img3path = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == var_presupuesto.PRE_ID).img3path;
            //  var_presupuesto.img4path = db.PRESUPUESTOS.SingleOrDefault(c => c.PRE_ID == var_presupuesto.PRE_ID).img4path;

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

                if (image1 != null)
                {
                    String path1 = LoadImage(image1, var_presupuesto);
                    var_presupuesto.img1path = path1;
                }

                if (image2 != null)
                {
                    String path2 = LoadImage(image2, var_presupuesto);
                    var_presupuesto.img2path = path2;
                }

                if (image3 != null)
                {
                    String path3 = LoadImage(image3, var_presupuesto);
                    var_presupuesto.img3path = path3;
                }

                if (image4 != null)
                {
                    String path4 = LoadImage(image4, var_presupuesto);
                    var_presupuesto.img4path = path4;
                }


                db.Entry(var_presupuesto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        string errorMessage = validationError.ErrorMessage;
                        return RedirectToAction("List");
                    }
                    return RedirectToAction("List");
                }
                return RedirectToAction("List");
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