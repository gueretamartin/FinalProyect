
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
    
public partial class TIPO_CLIENTE
{

    public TIPO_CLIENTE()
    {

        this.CLIENTES = new HashSet<CLIENTES>();

    }


    public int TC_ID { get; set; }

    public string TC_DESCRIPCION { get; set; }



    public virtual ICollection<CLIENTES> CLIENTES { get; set; }

}

}
