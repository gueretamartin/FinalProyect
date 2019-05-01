
namespace Octopus.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ESTADOS
    {
        public ESTADOS()
        {
            this.USUARIOS = new HashSet<USUARIOS>();
        }
    
        public int ESTADO { get; set; }
        public string ESTADO_DESC { get; set; }
    
        public virtual ICollection<USUARIOS> USUARIOS { get; set; }
    }
}
