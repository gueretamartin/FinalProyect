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
    
    public partial class MONEDAS
    {
        public MONEDAS()
        {
            this.VEHICULOS = new HashSet<VEHICULOS>();
        }
    
        public int MON_ID { get; set; }
        public string MON_DESCRIPCION { get; set; }
    
        public virtual ICollection<VEHICULOS> VEHICULOS { get; set; }
    }
}
