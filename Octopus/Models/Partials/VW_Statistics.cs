﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.WebPages;
using System.Web.Mvc;

namespace Octopus.Models
{
    
    public interface IVW_StatisticsMetadata
    {
        
    }
    
    [MetadataType(typeof(IVW_StatisticsMetadata))]
    public partial class VW_Statistics : IVW_StatisticsMetadata
    {
        private OctopusEntities db = new OctopusEntities();

        public int getCantidad(string tipo)
        {
            int cantidad = (int)db.VW_Statistics.FirstOrDefault(e => e.TIPO == tipo).CANTIDAD;
            return cantidad;

        }

    }
}