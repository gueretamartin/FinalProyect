using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octopus.Models;
using System.Web.SessionState;

namespace Octopus.Controllers
{
    public class ClientesController : Controller
    {
        private OctopusEntities db = new OctopusEntities();

        //
        // GET: /Clientes/

        public ActionResult Index()
        {
            return View(db.CLIENTES.ToList());
        }

        //
        // GET: /Clientes/Details/5

        public ActionResult Details(int id = 0)
        {
            CLIENTES clientes = db.CLIENTES.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            Session["CONDICIONIVA"] = clientes.TC_ID;
            
            return View(clientes);
        }

        //
        // GET: /Clientes/Create

        public ActionResult Create()
        {
            ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

            ViewBag.TD_ID = new SelectList(db.TIPO_DOCUMENTO, "TD_ID", "TD_DESCRIPCION");

            return View();  
            //USUARIOS user = @ViewBag.Usuario;
            //return RedirectToAction("Create");
            //return View(user);
        }

        
        //
        // POST: /Clientes/Create

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CLIENTES clientes)
        {
            //CHEQUEA SI EL DOCUMENTO YA FUE CARGADO
            if ((db.CLIENTES.Any(c => c.CLI_DOC.Equals(clientes.CLI_DOC)) && !String.IsNullOrEmpty(clientes.CLI_DOC))
                || (db.CLIENTES.Any(c => c.CLI_RI_CUIT.Equals(clientes.CLI_RI_CUIT)) && !String.IsNullOrEmpty(clientes.CLI_RI_CUIT)))
            {
                ModelState.AddModelError("CLI_DOC", "EL DNI/CUIT INGRESADO YA EXISTE");
                return View("Create");
            }

            //CHEQUEA QUE HAYA COMPLETADO LO NECESARIO EN ALGUN TIPO DE CONDICIÓN IVA
            if((clientes.TC_ID.Equals("CONSUMIDOR_FINAL") && (clientes.TD_ID == null || clientes.CLI_DOC == null))
                ||
               (clientes.TC_ID.Equals("RESPONSABLE_INSCRIPTO") && clientes.CLI_RI_CUIT == null))
            {
                ModelState.AddModelError("CLI_DOC", "DEBE REGISTRAR ALGÚN DOCUMENTO/CUIT");
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                CleanClient(clientes);
                db.CLIENTES.Add(clientes);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(clientes);
        }

        public void CleanClient(CLIENTES cliente)
        {
            if (cliente.TC_ID.Equals("CONSUMIDOR_FINAL"))
            {
                //cliente.CLI_RI_CUIT = null;
                //cliente.CLI_RI_RAZONSOCIAL = null;
                //cliente.CLI_RI_DIRECCION = null;
                //cliente.CLI_RI_LOCALIDAD = null;
                //cliente.CLI_RI_PROVINCIA = null;
                //cliente.CLI_RI_PAIS = null;
                //cliente.CLI_RI_CODPOSTAL = null;
                //cliente.TD_ID = null;
            }
            else
            {
                //cliente.TD_ID = null;
                //cliente.CLI_DOC = null;
                //cliente.CLI_CUIL = null;
                //cliente.CLI_NOMBRE = null;
                //cliente.CLI_APELLIDO = null;
                //cliente.CLI_EMAIL = null;
                //cliente.CLI_TELEFONO = null;
                //cliente.CLI_DIRECCION = null;
                //cliente.CLI_LOCALIDAD = null;
                //cliente.CLI_PROVINCIA = null;
                //cliente.CLI_PAIS = null;
                //cliente.CLI_CODPOSTAL = null;
            }
        }

        /*
        public ActionResult List()
        {
            return View(db.CLIENTES.ToList());
        }
         * */


        public ActionResult List(string searchClient)
        {
            var clientes = from c in db.CLIENTES
                           select c;

            //var clientes = db.CLIENTES;

            if (!String.IsNullOrEmpty(searchClient))
            {
                clientes = clientes.Where(c => c.CLI_NOMBRE.Contains(searchClient)
                    || c.CLI_APELLIDO.Contains(searchClient)
                    || c.CLI_DOC.Contains(searchClient)
                    || c.CLI_RI_RAZONSOCIAL.Contains(searchClient)
                    || c.CLI_RI_CUIT.Contains(searchClient)
                );
            }
            return View(clientes.ToList());
        }


        [HttpPost]
        public ActionResult Acciones(int cli_id, string Action)
        {
            if (Action == "Edit")
            {
                return RedirectToAction("Edit", new { cli_id = cli_id });
            }
            else if (Action == "Delete")
            {
                return RedirectToAction("Delete", new { cli_id = cli_id });
            }
            else
            {
                return View("List");
            }
        }

        //
        // GET: /Clientes/Edit/5

        public ActionResult Edit(int cli_id)
        {
            try
            {
                CLIENTES cliente = db.CLIENTES.SingleOrDefault(c => c.CLI_ID == cli_id);
                return View(cliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // POST: /Clientes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CLIENTES clientes)
        {
            clientes.TC_ID = int.Parse(Session["CONDICIONIVA"].ToString());
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(clientes);
        }

        //
        // GET: /Clientes/Delete/5

        public ActionResult Delete(int cli_id)
        {
            CLIENTES clientes = db.CLIENTES.Find(cli_id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.CLIENTES.Remove(clientes);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }

        //
        // POST: /Clientes/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLIENTES clientes = db.CLIENTES.Find(id);
            db.CLIENTES.Remove(clientes);
            db.SaveChanges();
            return RedirectToAction("List");
        }
          */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CLIENTES clientes)
        {
            if (ModelState.IsValid)
            {
                CLIENTES cliact = db.CLIENTES.Find(clientes.CLI_ID);
                db.CLIENTES.Remove(cliact);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(clientes);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}