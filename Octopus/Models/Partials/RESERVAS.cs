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
    
    public interface IReservasMetadata
    {
        //int PRE_ID { get; set; }


       
         
       
        //[Required(ErrorMessage = "CLIENTE: Seleccione un Cliente")]
        //int CLI_ID { get; set; }

        //[Required(ErrorMessage = "MARCA: Seleccione una Marca")]
        //int PRE_MARCA { get; set; }  

        //[Required(ErrorMessage = "MODELO: Campo Requerido")]
        //[StringLength(255, ErrorMessage = "MODELO: Se aceptan hasta 255 caracteres.")]
        //string PRE_MODELO { get; set; }

        //[Required(ErrorMessage = "AÑO: Campo Requerido")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "AÑO: Sólo se permiten números")]
        //[StringLength(30, ErrorMessage = "AÑO: Se aceptan hasta 30 números.")]
        //string PRE_ANIO { get; set; }

        //[Required(ErrorMessage = "VERSIÓN: Campo Requerido")]
        //[StringLength(30, ErrorMessage = "VERSIÓN: Se aceptan hasta 30 caracteres.")]
        //string PRE_VERSION { get; set; }

        //[Required(ErrorMessage = "PATENTE: Campo Requerido")]
        //[StringLength(10, ErrorMessage = "PATENTE: Se aceptan hasta 10 caracteres.")]
        //string PRE_PATENTE { get; set; }

        //[Required(ErrorMessage = "PRECIO: Campo Requerido")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "PRECIO: Sólo se permiten números")]
        //int PRE_PRECIO { get; set; }
      
    }
    
    [MetadataType(typeof(IReservasMetadata))]
    public partial class RESERVAS : IReservasMetadata
    {
        //public SelectList Estados_List { get; set; }
        //public SelectList Empleados_List { get; set; }
        //public SelectList Clientes_List { get; set; }
        //public SelectList Marcas_List { get; set; }

       

        public String FechaHastaR
        {
            get
            {
                string anio = FECHAS1.FEC_FECHA.ToString().Substring(6, 4);
                string mes = FECHAS1.FEC_FECHA.ToString().Substring(3, 2);
                string dia = FECHAS1.FEC_FECHA.ToString().Substring(0, 2);
                string fecha = anio + "/" + mes + "/" + dia;
                return fecha;
            }
        }

       
        public String FechaHastaEdicion
        {
            get
            {
                string anio = FECHAS1.FEC_FECHA.ToString().Substring(6, 4);
                string mes = FECHAS1.FEC_FECHA.ToString().Substring(3, 2);
                string dia = FECHAS1.FEC_FECHA.ToString().Substring(0, 2);
                string fecha = anio + "-" + mes + "-" + dia;
                return fecha;
            }
        }

    }
}