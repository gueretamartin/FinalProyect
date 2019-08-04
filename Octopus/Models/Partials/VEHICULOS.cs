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

        [StringLength(255, ErrorMessage = "MODELO: Se aceptan hasta 255 caracteres.")]
        string VEH_MODELO { get; set; }

        [StringLength(30, ErrorMessage = "VERSIÓN: Se aceptan hasta 30 caracteres.")]
        string VEH_VERSION { get; set; }

        [StringLength(30, ErrorMessage = "CILINDRADA: Se aceptan hasta 30 caracteres.")]
        string VEH_CILINDRADAS { get; set; }

        [StringLength(255, ErrorMessage = "COLOR: Se aceptan hasta 255 caracteres.")]
        string VEH_COLOR { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "KILÓMETROS: Sólo se permiten números")]
        [StringLength(30, ErrorMessage = "KILÓMETROS: Se aceptan hasta 30 números.")]
        string VEH_KILOMETROS { get; set; }

        [StringLength(255, ErrorMessage = "DETALLES: Se aceptan hasta 255 caracteres.")]
        string VEH_DETALLES { get; set; }

        [StringLength(50, ErrorMessage = "PUERTAS: Se aceptan hasta 50 caracteres.")]
        string VEH_PUERTAS { get; set; }

        Nullable<int> VEH_TIPOCOMBUSTIBLE { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "AÑO: Sólo se permiten números")]
        [StringLength(30, ErrorMessage = "AÑO: Se aceptan hasta 30 números.")]
        string VEH_AÑO { get; set; }

        [StringLength(10, ErrorMessage = "PATENTE: Se aceptan hasta 10 caracteres.")]
        string VEH_PATENTE { get; set; }

        Nullable<int> VEH_TIPOVEHICULO { get; set; }

        bool VEH_VIGENTE { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "PRECIO DE INGRESO: Sólo se permiten números")]
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

        [StringLength(150, ErrorMessage = "MODELO DE ESTEREO: Se aceptan hasta 150 caracteres.")]
        string VEH_STEREO_MODELO { get; set; }

        [StringLength(50, ErrorMessage = "CÓDIGO DE ESTEREO: Se aceptan hasta 50 caracteres.")]
        string VEH_STEREO_CODIGO { get; set; }
        
        Nullable<int> CLI_ID { get; set; }
        
        Nullable<int> EMP_ID { get; set; }
        
        Nullable<int> FEC_ID { get; set; }

        Nullable<int> MAR_ID { get; set; }

        Nullable<int> IMG_ID { get; set; }

        Nullable<int> ES_ID { get; set; }

        Nullable<int> MON_ID { get; set; }

    }
    
    [MetadataType(typeof(IVehiculosMetadata))]
    public partial class VEHICULOS : IVehiculosMetadata
    {

        public string VEH_PAT_MARCA_MODELO
        {
            get
            {
                return String.Format("Patente: " + VEH_PATENTE + " | " + MARCAS.MAR_DESCRIPCION + " " + VEH_MODELO + " " + VEH_VERSION);
            }
        }

        public string VEH_FIRSTS_DETAILS
        {
            get
            {
                return String.Format(VEH_PATENTE + " -- " + MARCAS.MAR_DESCRIPCION + " " + VEH_MODELO + " " + VEH_VERSION
                    + " -- " + VEH_AÑO + " -- " + TIPO_COMBUSTIBLES.TCOM_DESCRIPCION);
            }
        }

        public string VEH_SECONDS_DETAILS
        {
            get
            {
                return String.Format("COLOR " + VEH_COLOR + " -- " + VEH_KILOMETROS + " KMs -- " + VEH_PUERTAS + " PUERTAS");
            }
        }
    }
}