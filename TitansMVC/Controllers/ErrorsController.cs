using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TitansMVC.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Http404(HttpException exception)
        {
            Response.StatusCode = 404;
            Response.ContentType = "text/html";
            return View(exception);
        }
        //ok

        public ActionResult Http500(Exception exception)
        {
            Response.StatusCode = 500;
            Response.ContentType = "text/html";
            return View(exception);
        }
    }
}
