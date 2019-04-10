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
    
    public interface IClientesMetadata
    {
        int CLI_ID { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "NRO. DOCUMENTO: Sólo se permiten números.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [StringLength(9, ErrorMessage="NRO. DOCUMENTO: Se aceptan hasta 9 números.")]
        string CLI_DOC { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CUIL: Sólo se permiten números.")]
        [StringLength(12, ErrorMessage = "CUIL: Se aceptan hasta 12 números.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        string CLI_CUIL { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "NOMBRE: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "NOMBRE: Se aceptan hasta 50 letras.")]
        string CLI_NOMBRE { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "APELLIDO: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "APELLIDO: Se aceptan hasta 50 letras.")]
        string CLI_APELLIDO { get; set; }

        [EmailAddress(ErrorMessage = "E - MAIL: Formato de correo electrónico incorrecto.")]
        [StringLength(50, ErrorMessage = "E - MAIL: Se aceptan hasta 50 letras.")]
        string CLI_EMAIL { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "TELÉFONO: Sólo se permiten números.")]
        [StringLength(15, ErrorMessage = "TELÉFONO: Se aceptan hasta 15 números.")]
        string CLI_TELEFONO { get; set; }

        [StringLength(100, ErrorMessage = "DIRECCIÓN: Se aceptan hasta 100 letras.")]
        string CLI_DIRECCION { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "LOCALIDAD - Sólo se permiten letras.")]
        [StringLength(100, ErrorMessage = "LOCALIDAD: Se aceptan hasta 100 letras.")]
        string CLI_LOCALIDAD { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PROVINCIA: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "PROVINCIA: Se aceptan hasta 50 letras.")]
        string CLI_PROVINCIA { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PAÍS: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "PAÍS: Se aceptan hasta 50 letras.")]
        string CLI_PAIS { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CUIT: Sólo se permiten números.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [StringLength(12, ErrorMessage = "CUIT: Se aceptan hasta 12 números.")]
        string CLI_RI_CUIT { get; set; }

        [StringLength(100, ErrorMessage = "RAZÓN SOCIAL: Se aceptan hasta 100 letras.")]
        string CLI_RI_RAZONSOCIAL { get; set; }

        [StringLength(100, ErrorMessage = "DIRECCIÓN: Se aceptan hasta 100 letras.")]
        string CLI_RI_DIRECCION { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "LOCALIDAD: Sólo se permiten letras.")]
        [StringLength(100, ErrorMessage = "LOCALIDAD: Se aceptan hasta 100 letras.")]
        string CLI_RI_LOCALIDAD { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PROVINCIA: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "PROVINCIA: Se aceptan hasta 50 letras.")]
        string CLI_RI_PROVINCIA { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "PAÍS: Sólo se permiten letras.")]
        [StringLength(50, ErrorMessage = "PAÍS: Se aceptan hasta 50 letras.")]
        string CLI_RI_PAIS { get; set; }

        [Required(ErrorMessage = "TIPO CLIENTE: Requerido.")]
        Nullable<int> TC_ID { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CÓDIGO POSTAL: Sólo se permiten números.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [StringLength(6, ErrorMessage = "CÓDIGO POSTAL: Se aceptan hasta 6 números.")]
        string CLI_CODPOSTAL { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "CÓDIGO POSTAL: Sólo se permiten números.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [StringLength(6, ErrorMessage = "CÓDIGO POSTAL: Se aceptan hasta 6 números.")]
        string CLI_RI_CODPOSTAL { get; set; }

        Nullable<int> TD_ID { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "TELÉFONO CONTACTO: Sólo se permiten números.")]
        [StringLength(15, ErrorMessage = "TELÉFONO CONTACTO: Se aceptan hasta 15 números.")]
        string CLI_RI_TELEFONO { get; set; }

        [EmailAddress(ErrorMessage = "E - MAIL CONTACTO: Formato de correo electrónico incorrecto.")]
        [StringLength(100, ErrorMessage = "E - MAIL CONTACTO: Se aceptan hasta 100 letras")]
        string CLI_RI_EMAIL { get; set; }
    }
    
    [MetadataType(typeof(IClientesMetadata))]
    public partial class CLIENTES : IClientesMetadata
    {

        public string CLI_LOCALIDAD_JERARQUIA
        {
            get
            {
                return String.Format(CLI_LOCALIDAD + " - " + CLI_PROVINCIA + " - " + CLI_PAIS);
            }
        }

        public string CLI_RI_LOCALIDAD_JERARQUIA
        {
            get
            {
                return String.Format(CLI_RI_LOCALIDAD + " - " + CLI_RI_PROVINCIA + " - " + CLI_RI_PAIS);
            }
        }

    }
}