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
        [StringLength(50, ErrorMessage = "USUARIO: Se aceptan hasta 50 letras.")]
        string Usuario { get; set; }

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

        public SelectList Roles_List { get; set; }
        public SelectList Estados_List { get; set; }

    }
}