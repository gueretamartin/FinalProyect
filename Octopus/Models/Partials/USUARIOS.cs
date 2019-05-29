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
    
    public interface IUsuariosMetadata
    {
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "USUARIO: Sólo se permiten letras.")]
        [Required(ErrorMessage = "Usuario - Campo Requerido")]
        [StringLength(50, ErrorMessage = "USUARIO: Se aceptan hasta 50 letras.")]
        string Usuario { get; set; }


        [Required(ErrorMessage = "Contraseña - Campo Requerido")]
        string Contraseña { get; set; }
    }
    
    [MetadataType(typeof(IUsuariosMetadata))]
    public partial class USUARIOS : IUsuariosMetadata
    {
        private OctopusEntities db = new OctopusEntities();

        public EMPLEADOS getEmpleado(String username)
        {   
            EMPLEADOS emp = db.EMPLEADOS.FirstOrDefault(e => e.EMP_USUARIO == username);
            return emp;
        }

        public EMPLEADOS getEmpleadoPorId(int? emp_id)
        {
            EMPLEADOS emp = db.EMPLEADOS.FirstOrDefault(e => e.EMP_ID == emp_id);
            return emp;
        }


     
        public SelectList Roles_List { get; set; }
        public SelectList Estados_List { get; set; }
        public SelectList Empleados_List { get; set; }

        public  int? emp_id { get; set; }


        private EMPLEADOS emp; 
        public EMPLEADOS Emp
        {
            get
            {
                emp = db.EMPLEADOS.SingleOrDefault(c => c.EMP_ID == emp_id);
                return emp;
            }
            set
            {
                emp = db.EMPLEADOS.SingleOrDefault(c => c.EMP_ID == emp_id);
               
            }
        }


       
    }
}