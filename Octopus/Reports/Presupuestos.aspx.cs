using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace Octopus.Reports
{
    public partial class Presupuestos1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {}
           
        protected void Print(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string deviceInfo = "<DeviceInfo>"
                                + "<OutputFormat>EMF</OutputFormat>"
                                + "<PageWidth>8.5in</PageWidth>"
                                + "<PageHeight>11in</PageHeight>"
                                + "<MarginTop>0.25in</MarginTop>"
                                + "<MarginLeft>0.25in</MarginLeft>"
                                + "<MarginRight>0.25in</MarginRight>"
                                + "<MarginBottom>0.25in</MarginBottom>"
                                + "</DeviceInfo>";
            byte[] bytes = ReportViewer1.LocalReport.Render((sender as Button).CommandName, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=Presupuesto." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}
