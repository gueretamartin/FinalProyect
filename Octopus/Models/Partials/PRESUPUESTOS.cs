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
    
    public interface IPresupuestosMetadata
    {
        int PRE_ID { get; set; }
        int EMP_ID { get; set; }
        [Required(ErrorMessage = "Cliente - Campo Requerido")]
        int CLI_ID { get; set; }
        [Required(ErrorMessage = "Marca - Campo Requerido")]
        int PRE_MARCA { get; set; }  
        [Required(ErrorMessage = "Modelo - Campo Requerido")]
        string PRE_MODELO { get; set; }
        [Required(ErrorMessage = "Año - Campo Requerido")]
        string PRE_ANIO { get; set; }
        [Required(ErrorMessage = "Versión - Campo Requerido")]
        string PRE_VERSION { get; set; }
        [Required(ErrorMessage = "Patente - Campo Requerido")]
        string PRE_PATENTE { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]

         [Required(ErrorMessage = "Precio - Campo Requerido")]
        decimal PRE_PRECIO { get; set; }

    }
    
    [MetadataType(typeof(IPresupuestosMetadata))]
    public partial class PRESUPUESTOS : IPresupuestosMetadata
    {
        public SelectList Estados_List { get; set; }
        public SelectList Empleados_List { get; set; }
        public SelectList Clientes_List { get; set; }
        public SelectList Marcas_List { get; set; }

        public String FechaCreacion
        {
            get {
                string anio = FEC_ID.ToString().Substring(0, 4);
                string mes = FEC_ID.ToString().Substring(4, 2);
                string dia = FEC_ID.ToString().Substring(6, 2);
                string fecha = anio + "/" + mes + "/" + dia;
                return fecha;
            }
        }

        public String FechaHasta
        {
            get
            {
                string anio = PRE_FECHA_FIN.ToString().Substring(0, 4);
                string mes = PRE_FECHA_FIN.ToString().Substring(4, 2);
                string dia = PRE_FECHA_FIN.ToString().Substring(6, 2);
                string fecha = anio + "/" + mes + "/" + dia;
                return fecha;
            }
        }

        public String FechaHastaEdicion
        {
            get
            {
                string anio = PRE_FECHA_FIN.ToString().Substring(0, 4);
                string mes = PRE_FECHA_FIN.ToString().Substring(4, 2);
                string dia = PRE_FECHA_FIN.ToString().Substring(6, 2);
                string fecha = anio + "-" + mes + "-" + dia;
                return fecha;
            }
        }

    }
}