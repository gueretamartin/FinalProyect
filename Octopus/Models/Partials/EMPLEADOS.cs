using Octopus.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.WebPages;
using System.Web.Mvc;

namespace Octopus.Models
{
    
    public interface IEmpleadosMetadata
    {
        
        [Required(ErrorMessage = "DNI: Campo Requerido")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "DNI: Sólo se permiten letras y numeros")]
        string EMP_DNI { get; set; }

        [Required(ErrorMessage = "NOMBRE: Campo Requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "NOMBRE: Sólo se permiten letras")]
        string EMP_NOMBRE { get; set; }

        [Required(ErrorMessage = "APELLIDO: Campo Requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "APELLIDO: Sólo se permiten letras")]
        string EMP_APELLIDO { get; set; }

        [Required(ErrorMessage = "E-MAIL: Campo Requerido")]
        [EmailAddress(ErrorMessage = "E-MAIL: Ingrese un E-MAIL válido. (example@example.com)")]
        string EMP_EMAIL { get; set; }

        [Required(ErrorMessage = "SUCURSAL: Campo Requerido")]
        int SUC_ID { get; set; }

        [Required(ErrorMessage = "CARGO: Campo Requerido")]
        int EMP_CARGO { get; set; }

        
    }
    
    [MetadataType(typeof(IEmpleadosMetadata))]
    public partial class EMPLEADOS : IEmpleadosMetadata
    {
        private OctopusEntities db = new OctopusEntities();

        public SelectList Sucursales_List { get; set; }
        public SelectList Cargos_List { get; set; }
       

        public string NombreParaMostrar
        {
            get { return EMP_APELLIDO + ", " + EMP_NOMBRE + " (DNI: " + EMP_DNI + ")"
                ;}
        
        }

        public string SUC_DESC
        {
            get
            {
                return db.SUCURSALES.FirstOrDefault(c => c.SUC_ID == SUC_ID).SUC_DESCRIP;
            }
        }

        public string EMP_APELLIDO_NOMBRE
        {
            get
            {
                return EMP_APELLIDO + " " + EMP_NOMBRE;
            }

        }      
      
    }


       
    
}