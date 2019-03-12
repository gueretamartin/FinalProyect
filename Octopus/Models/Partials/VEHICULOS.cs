using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Octopus.Models
{
    public interface IVehiculosMetadata
    {
        int VEH_ID { get; set; }

        Nullable<int> SUC_ID { get; set; }

        [Required(ErrorMessage = "Marca Requerida")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Marca - Sólo se permiten letras")]
        string VEH_MARCA { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Modelo - Sólo se permiten letras")]
        string VEH_MODELO { get; set; }
        string VEH_VERSION { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Cilindradas - Sólo se permiten números")]
        Nullable<int> VEH_CILINDRADAS { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Color - Sólo se permiten letras")]
        string VEH_COLOR { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "KMs - Sólo se permiten números")]
        Nullable<int> VEH_KILOMETROS { get; set; }
        string VEH_DETALLES { get; set; }
        string VEH_PUERTAS { get; set; }
        string VEH_TIPOCOMBUSTIBLE { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Año - Sólo se permiten números")]
        Nullable<int> VEH_AÑO { get; set; }

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Patente - Sólo se permiten letras y numeros")]
        [Required(ErrorMessage = "Patente - Campo Requerido")]
        string VEH_PATENTE { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Tipo de Vehículo - Sólo se permiten letras")]
        string VEH_TIPOVEHICULO { get; set; }
        Nullable<bool> VEH_VIGENTE { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Precio - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        Nullable<int> VEH_PRECIO_INGRESO { get; set; }
        bool VEH_CIERRE { get; set; }
        bool VEH_LLANTAS { get; set; }
        bool VEH_AIRBAG { get; set; }
        bool VEH_AIRE { get; set; }
        bool VEH_LEVANTA_VIDRIOS { get; set; }
        bool VEH_FRENOS_ABS { get; set; }
        bool VEH_NEBLINEROS { get; set; }
        bool VEH_ESPEJOS_AUTOM { get; set; }
        bool VEH_ASIENTOS_CUERO { get; set; }
        bool VEH_RUEDA_AUXILIAR { get; set; }
        string VEH_STEREO_MODELO { get; set; }
        string VEH_STEREO_CODIGO { get; set; }
        int CLI_ID { get; set; }
        int EMP_ID { get; set; }
        int FEC_ID { get; set; }

    }
    
    [MetadataType(typeof(IVehiculosMetadata))]
    public partial class VEHICULOS : IVehiculosMetadata
    {

        public string VEH_PAT_MARCA_MODELO
        {
            get
            {
                return String.Format(VEH_PATENTE + " ||| " + VEH_MARCA + " " + VEH_MODELO + " " + VEH_VERSION);
            }
        }
    }
}