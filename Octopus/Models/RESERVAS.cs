//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Octopus.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RESERVAS
    {
        public int RES_ID { get; set; }
        public Nullable<int> VEH_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> RES_SENIA { get; set; }
        public Nullable<int> RES_VALOR_PACTADO { get; set; }
        public Nullable<System.DateTime> RES_INICIO { get; set; }
        public Nullable<System.DateTime> RES_FIN { get; set; }
        public Nullable<bool> RES_ESTADO { get; set; }
        public Nullable<int> FEC_ID { get; set; }
        public string CLIENTE_NOMBRE { get; set; }
        public string CLIENTE_APELLIDO { get; set; }
        public Nullable<int> CLIENTE_DOC { get; set; }
    
        public virtual EMPLEADOS EMPLEADOS { get; set; }
        public virtual FECHAS FECHAS { get; set; }
    }
}
