using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Octopus.Models
{
    
    public interface IClientesMetadata
    {
        int CLI_ID { get; set; }

        [Required(ErrorMessage = "Tipo de IVA Requerido")]
        string CLI_CONDICIONIVA { get; set; }
        string CLI_CF_TIPODOC { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "DOCUMENTO - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [MaxLength(9, ErrorMessage="DOCUMENTO: Se aceptan hasta 9 números")]
        string CLI_CF_DOC { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CUIL - Sólo se permiten números")]
        [MaxLength(12, ErrorMessage = "CUIL: Se aceptan hasta 12 números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        string CLI_CF_CUIL { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "NOMBRE - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "NOMBRE: Se aceptan hasta 50 letras")]
        string CLI_CF_NOMBRE { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "APELLIDO - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "APELLIDO: Se aceptan hasta 50 letras")]
        string CLI_CF_APELLIDO { get; set; }

        [MaxLength(50, ErrorMessage = "EMAIL: Se aceptan hasta 50 letras")]
        string CLI_CF_EMAIL { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "TELÉFONO - Sólo se permiten números")]
        [MaxLength(15, ErrorMessage = "TELÉFONO: Se aceptan hasta 15 números")]
        string CLI_CF_TELEFONO { get; set; }

        [MaxLength(100, ErrorMessage = "DIRECCIÓN: Se aceptan hasta 100 letras")]
        string CLI_CF_DIRECCION { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "LOCALIDAD - Sólo se permiten letras")]
        [MaxLength(100, ErrorMessage = "LOCALIDAD: Se aceptan hasta 100 letras")]
        string CLI_CF_LOCALIDAD { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PROVINCIA - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "PROVINCIA: Se aceptan hasta 50 letras")]
        string CLI_CF_PROVINCIA { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PAIS - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "PAÍS: Se aceptan hasta 50 letras")]
        string CLI_CF_PAIS { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CÓDIGO POSTAL - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [MaxLength(6, ErrorMessage = "COD POSTAL: Se aceptan hasta 6 números")]
        Nullable<int> CLI_CF_CODPOSTAL { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CUIT - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [MaxLength(12, ErrorMessage = "CUIT: Se aceptan hasta 12 números")]
        string CLI_RI_CUIT { get; set; }

        [MaxLength(100, ErrorMessage = "RAZÓN SOCIAL: Se aceptan hasta 100 letras")]
        string CLI_RI_RAZONSOCIAL { get; set; }

        [MaxLength(100, ErrorMessage = "DIRECCIÓN: Se aceptan hasta 100 letras")]
        string CLI_RI_DIRECCION { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "LOCALIDAD - Sólo se permiten letras")]
        [MaxLength(100, ErrorMessage = "LOCALIDAD: Se aceptan hasta 100 letras")]
        string CLI_RI_LOCALIDAD { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PROVINCIA - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "PROVINCIA: Se aceptan hasta 50 letras")]
        string CLI_RI_PROVINCIA { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PAIS - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "PAÍS: Se aceptan hasta 50 letras")]
        string CLI_RI_PAIS { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CÓDIGO POSTAL - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [MaxLength(6, ErrorMessage = "COD POSTAL: Se aceptan hasta 6 números")]
        Nullable<int> CLI_RI_CODPOSTAL { get; set; }
        string CLI_RI_CON_TIPODOC { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CONTACTO: DOCUMENTO - Sólo se permiten números")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [MaxLength(9, ErrorMessage = "DOCUMENTO: Se aceptan hasta 9 números")]
        string CLI_RI_CON_DOC { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "NOMBRE - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "NOMBRE: Se aceptan hasta 50 letras")]
        string CLI_RI_CON_NOMBRE { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "APELLIDO - Sólo se permiten letras")]
        [MaxLength(50, ErrorMessage = "APELLIDO: Se aceptan hasta 50 letras")]
        string CLI_RI_CON_APELLIDO { get; set; }

        [MaxLength(100, ErrorMessage = "MAIL: Se aceptan hasta 100 letras")]
        string CLI_RI_CON_EMAIL { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CONTACTO: TELÉFONO - Sólo se permiten números")]
        [MaxLength(15, ErrorMessage = "TELÉFONO: Se aceptan hasta 15 números")]
        string CLI_RI_CON_TELEFONO { get; set; }
    }
    
    [MetadataType(typeof(IClientesMetadata))]
    public partial class CLIENTES : IClientesMetadata
    {
        public enum TIPO_DOC
        {
            DNI,
            LE,
            PASAPORTE
        }

        public enum CLI_TIPO_IVA
        {
            [Display(Name="CONSUMIDOR FINAL")]
            CONSUMIDOR_FINAL = 1,

            [Display(Name = "RESPONSABLE INSCRIPTO")]
            RESPONSABLE_INSCRIPTO = 2
        }

        

        public string CLI_CF_DNI_APELLIDO_NOMBRE 
        { 
            get 
            {
                return String.Format(String.Format("{0:n0}", Int32.Parse(CLI_CF_DOC)) + " - " + CLI_CF_APELLIDO + " " + CLI_CF_NOMBRE); 
            } 
        }

        public string CLI_CF_LOCALIDAD_JERARQUIA
        {
            get
            {
                return String.Format(CLI_CF_LOCALIDAD + " - " + CLI_CF_PROVINCIA + " - " + CLI_CF_PAIS);
            }
        }

        public string CLI_RI_DNI_APELLIDO_NOMBRE
        {
            get
            {
                return String.Format(String.Format("{0:n0}", Int32.Parse(CLI_CF_DOC)) + " - " + CLI_CF_APELLIDO + " " + CLI_CF_NOMBRE);
            }
        }

        public string CLI_RI_LOCALIDAD_JERARQUIA
        {
            get
            {
                return String.Format(CLI_CF_LOCALIDAD + " - " + CLI_CF_PROVINCIA + " - " + CLI_CF_PAIS);
            }
        }

    }
}