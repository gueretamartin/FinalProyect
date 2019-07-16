using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Spire.Pdf;
using System.Drawing;
using Spire.Pdf.Widget;
using Spire.Pdf.Fields;
using System.Threading;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.Graphics;

namespace Octopus.Controllers
{
    public class ManualController : Controller
    {
        //
        // GET: /Manual/
        public void OpenManual()
        {
            Response.ContentType = "Application/pdf";
            Response.TransmitFile(Server.MapPath("~/Manuals/") + "manual_de_usuario.pdf");
            //PdfDocument doc = new PdfDocument();
            //System.Diagnostics.Process.Start(Server.MapPath("~/Manual/") + "manual_de_usuario.pdf");
            //return RedirectToAction("Home/Home");
        }
    }
}