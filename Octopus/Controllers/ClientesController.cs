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

        // GET: /Clientes/List?searchClient=[Parámetro]
        // LEVANTA LA VISTA DE LISTADO DE CLIENTES O LISTA DE CLIENTES QUE CONTIENEN CON EL PARÁMETRO PASADO
        public ActionResult List(string searchClient)
        {
            try
            {
                var clientes = from c in db.CLIENTES where c.ES_ID == 1 select c;

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
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }



        // GET: /Clientes/Details/[Parámetro]
        // LEVANTA LA VISTA DE DETALLE DEL CLIENTE PASADO POR PARÁMETRO
        public ActionResult Details(int id = 0)
        {
            try
            {
                CLIENTES clientes = db.CLIENTES.Find(id);

                if(clientes.ES_ID != 1)
                {
                    return RedirectToAction("Home", "Home");
                }

                Session["CONDICIONIVA"] = clientes.TC_ID;

                return View(clientes);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }



        // GET: /Clientes/Create
        // LEVANTA LA VISTA DE CREAR CLIENTE
        public ActionResult Create()
        {
            try
            {
                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION");

                ViewBag.TD_ID = new SelectList(db.TIPO_DOCUMENTO, "TD_ID", "TD_DESCRIPCION");

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }

        

        // POST: /Clientes/Create
        // VALIDA Y CREA EL CLIENTE QUE DE LA VISTA DE CREAR CLIENTE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CLIENTES var_cliente)
        {
            try
            {
                ESTADOS estado_cli = db.ESTADOS.SingleOrDefault(c => c.ES_ID.Equals(1));

                var_cliente.ES_ID = estado_cli.ES_ID;

                #region VALIDACIÓN DE TIPO DE CLIENTE, CLIENTE EXISTENTE Y CAMPOS VACÍOS
                if (!String.IsNullOrEmpty(var_cliente.TC_ID.ToString()))
                {
                    if (var_cliente.TC_ID.Equals(1))
                    {
                        var cli = from c in db.CLIENTES where c.ES_ID == 1 select c;

                        cli = cli.Where(c => c.CLI_CUIL.Contains(var_cliente.CLI_CUIL));

                        if (cli.Count() != 0)
                        {
                            ModelState.AddModelError("CLI_CUIL", "CUIL: Ya existe.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.TD_ID.ToString()))
                        {
                            ModelState.AddModelError("TD_ID", "TIPO DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_DOC))
                        {
                            ModelState.AddModelError("CLI_DOC", "NRO. DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_NOMBRE))
                        {
                            ModelState.AddModelError("CLI_NOMBRE", "NOMBRE: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_APELLIDO))
                        {
                            ModelState.AddModelError("CLI_APELLIDO", "APELLIDO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_PAIS))
                        {
                            ModelState.AddModelError("CLI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_CUIL))
                        {
                            ModelState.AddModelError("CLI_CUIL", "CUIL: Está vacío.");
                            return this.Create();
                        }
                    }
                    else
                    {
                        var cli = from c in db.CLIENTES where c.ES_ID == 1 select c;

                        cli = cli.Where(c => c.CLI_RI_CUIT.Contains(var_cliente.CLI_RI_CUIT));

                        if (cli.Count() != 0)
                        {
                            ModelState.AddModelError("CLI_RI_CUIT", "CUIT: Ya existe.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_CUIT))
                        {
                            ModelState.AddModelError("CLI_RI_CUIT", "CUIT: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_RAZONSOCIAL))
                        {
                            ModelState.AddModelError("CLI_RI_RAZONSOCIAL", "RAZÓN SOCIAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_RI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_RI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_RI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_PAIS))
                        {
                            ModelState.AddModelError("CLI_RI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_RI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_RI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_RI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.TD_ID.ToString()))
                        {
                            ModelState.AddModelError("TD_ID", "TIPO DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_DOC))
                        {
                            ModelState.AddModelError("CLI_DOC", "NRO. DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_NOMBRE))
                        {
                            ModelState.AddModelError("CLI_NOMBRE", "NOMBRE: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_APELLIDO))
                        {
                            ModelState.AddModelError("CLI_APELLIDO", "APELLIDO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_PAIS))
                        {
                            ModelState.AddModelError("CLI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(var_cliente.CLI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("TC_ID", "TIPO DE CLIENTE: Debe seleccionar un tipo de cliente.");
                    return this.Create();
                }
                #endregion

                var_cliente = ToUpperClient(var_cliente);

                var_cliente.CLI_ALTA = DateTime.Now.Date;

                if (ModelState.IsValid)
                {
                    db.CLIENTES.Add(var_cliente);
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

        // TRANSFOMA TODOS LOS CAMPOS DEL CLIENTE A MAYÚSCULA
        private CLIENTES ToUpperClient(CLIENTES var_cliente)
        {
            try
            {
                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_RAZONSOCIAL))
                {
                    var_cliente.CLI_RI_RAZONSOCIAL = var_cliente.CLI_RI_RAZONSOCIAL.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_DIRECCION))
                {
                    var_cliente.CLI_RI_DIRECCION = var_cliente.CLI_RI_DIRECCION.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_LOCALIDAD))
                {
                    var_cliente.CLI_RI_LOCALIDAD = var_cliente.CLI_RI_LOCALIDAD.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_PROVINCIA))
                {
                    var_cliente.CLI_RI_PROVINCIA = var_cliente.CLI_RI_PROVINCIA.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_PAIS))
                {
                    var_cliente.CLI_RI_PAIS = var_cliente.CLI_RI_PAIS.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_RI_EMAIL))
                {
                    var_cliente.CLI_RI_EMAIL = var_cliente.CLI_RI_EMAIL.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_NOMBRE))
                {
                    var_cliente.CLI_NOMBRE = var_cliente.CLI_NOMBRE.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_APELLIDO))
                {
                    var_cliente.CLI_APELLIDO = var_cliente.CLI_APELLIDO.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_DIRECCION))
                {
                    var_cliente.CLI_DIRECCION = var_cliente.CLI_DIRECCION.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_LOCALIDAD))
                {
                    var_cliente.CLI_LOCALIDAD = var_cliente.CLI_LOCALIDAD.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_PROVINCIA))
                {
                    var_cliente.CLI_PROVINCIA = var_cliente.CLI_PROVINCIA.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_PAIS))
                {
                    var_cliente.CLI_PAIS = var_cliente.CLI_PAIS.ToUpper();
                }

                if (!String.IsNullOrEmpty(var_cliente.CLI_EMAIL))
                {
                    var_cliente.CLI_EMAIL = var_cliente.CLI_EMAIL.ToUpper();
                }

                return var_cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // GET: /Clientes/Edit/(Parámetro)
        // LEVANTA LA VISTA DE DETALLE DEL CLIENTE PASADO POR PARÁMETRO
        public ActionResult Edit(int cli_id)
        {
            try
            {
                CLIENTES cliente = db.CLIENTES.SingleOrDefault(c => c.CLI_ID == cli_id);

                if (cliente.ES_ID != 1)
                {
                    return RedirectToAction("Home", "Home");
                }

                ViewBag.TC_ID = new SelectList(db.TIPO_CLIENTE, "TC_ID", "TC_DESCRIPCION", cliente.TC_ID);

                ViewBag.TD_ID = new SelectList(db.TIPO_DOCUMENTO, "TD_ID", "TD_DESCRIPCION", cliente.TD_ID);
                
                return View(cliente);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }


        // POST: /Clientes/Edit
        // VALIDA Y EDITA EL CLIENTE QUE DE LA VISTA DE EDITAR CLIENTE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CLIENTES clientes)
        {
            try
            {
                TIPO_CLIENTE tipo_cli = db.TIPO_CLIENTE.SingleOrDefault(c => c.TC_DESCRIPCION.Equals(clientes.TIPO_CLIENTE.TC_DESCRIPCION));

                clientes.TC_ID = tipo_cli.TC_ID;

                clientes.TIPO_CLIENTE = null;

                #region VALIDACIÓN DE TIPO DE CLIENTE, CLIENTE EXISTENTE Y CAMPOS VACÍOS
                if (!String.IsNullOrEmpty(clientes.TC_ID.ToString()))
                {
                    if (clientes.TC_ID.Equals(1))
                    {
                        if (String.IsNullOrEmpty(clientes.TD_ID.ToString()))
                        {
                            ModelState.AddModelError("TD_ID", "TIPO DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_DOC))
                        {
                            ModelState.AddModelError("CLI_DOC", "NRO. DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_NOMBRE))
                        {
                            ModelState.AddModelError("CLI_NOMBRE", "NOMBRE: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_APELLIDO))
                        {
                            ModelState.AddModelError("CLI_APELLIDO", "APELLIDO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_PAIS))
                        {
                            ModelState.AddModelError("CLI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_CUIL))
                        {
                            ModelState.AddModelError("CLI_CUIL", "CUIL: Está vacío.");
                            return this.Create();
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(clientes.CLI_RI_CUIT))
                        {
                            ModelState.AddModelError("CLI_RI_CUIT", "CUIT: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_RAZONSOCIAL))
                        {
                            ModelState.AddModelError("CLI_RI_RAZONSOCIAL", "RAZÓN SOCIAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_RI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_RI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_RI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_PAIS))
                        {
                            ModelState.AddModelError("CLI_RI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_RI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_RI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_RI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.TD_ID.ToString()))
                        {
                            ModelState.AddModelError("TD_ID", "TIPO DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_DOC))
                        {
                            ModelState.AddModelError("CLI_DOC", "NRO. DOCUMENTO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_NOMBRE))
                        {
                            ModelState.AddModelError("CLI_NOMBRE", "NOMBRE: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_APELLIDO))
                        {
                            ModelState.AddModelError("CLI_APELLIDO", "APELLIDO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_DIRECCION))
                        {
                            ModelState.AddModelError("CLI_DIRECCION", "DIRECCIÓN: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_LOCALIDAD))
                        {
                            ModelState.AddModelError("CLI_LOCALIDAD", "LOCALIDAD: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_PROVINCIA))
                        {
                            ModelState.AddModelError("CLI_PROVINCIA", "PROVINCIA: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_PAIS))
                        {
                            ModelState.AddModelError("CLI_PAIS", "PAÍS: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_TELEFONO))
                        {
                            ModelState.AddModelError("CLI_TELEFONO", "TELÉFONO: Está vacío.");
                            return this.Create();
                        }

                        if (String.IsNullOrEmpty(clientes.CLI_CODPOSTAL))
                        {
                            ModelState.AddModelError("CLI_CODPOSTAL", "CÓDIGO POSTAL: Está vacío.");
                            return this.Create();
                        }
                    }
                }
                #endregion

                clientes = ToUpperClient(clientes);

                if (ModelState.IsValid)
                {
                    db.Entry(clientes).State = EntityState.Modified;
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

        //POST: 
        // CONTROLADOR DE ACCIONES DE BOTONES PARA LEVANTAR LA VISTA DETERMINADA
        [HttpPost]
        public ActionResult Acciones(int cli_id, string Action)
        {
            try
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
            catch (Exception ex)
            {
                return RedirectToAction("Home", "Home");
            }
        }


        // POST:
        // // VALIDA Y CAMBIA EL ESTADO DEL CLIENTE A INACTIVO 
        public ActionResult Delete(int cli_id)
        {
            try
            {
                CLIENTES clientes = db.CLIENTES.Find(cli_id);

                if (clientes == null)
                {
                    return RedirectToAction("Home", "Home");
                }


                if (clientes.ES_ID != 1)
                {
                    return RedirectToAction("Home", "Home");
                }

                clientes.ES_ID = 2;

                if (ModelState.IsValid)
                {
                    db.Entry(clientes).State = EntityState.Modified;
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}