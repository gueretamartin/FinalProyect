using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Octopus.Models
{
    public interface IVentasMetadata
    {
        

    }

    [MetadataType(typeof(IVentasMetadata))]
    public partial class VENTAS : IVentasMetadata
    {
        public String FechaVentaMostrar
        {
            get
            {
                string anio = FEC_ID.ToString().Substring(0, 4);
                string mes = FEC_ID.ToString().Substring(4, 2);
                string dia = FEC_ID.ToString().Substring(6, 2);
                string fecha = anio + "/" + mes + "/" + dia;
                return fecha;
            }
        }
    }
}