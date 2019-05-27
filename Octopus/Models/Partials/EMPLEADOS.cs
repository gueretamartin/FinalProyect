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
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "DNI - Sólo se permiten letras y numeros")]
        [Required(ErrorMessage = "DNI - Campo Requerido")]
        string EMP_DNI { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Nombre - Sólo se permiten letras")]
        [Required(ErrorMessage = "Nombre - Campo Requerido")]
        string EMP_NOMBRE { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Apellido - Sólo se permiten letras")]
        [Required(ErrorMessage = "Apellido - Campo Requerido")]
        string EMP_APELLIDO { get; set; }

        [Required(ErrorMessage = "E-Mail - Campo Requerido")]
        string EMP_EMAIL { get; set; }

        [Required(ErrorMessage = "Sucursal - Campo Requerido")]
        int SUC_ID { get; set; }
        
    }
    
    [MetadataType(typeof(IEmpleadosMetadata))]
    public partial class EMPLEADOS : IEmpleadosMetadata
    {
        private OctopusEntities1 db = new OctopusEntities1();

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
      
      
    }


       
    
}