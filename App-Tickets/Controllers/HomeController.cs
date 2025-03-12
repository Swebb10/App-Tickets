using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App_Tickets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        [Authorize]
        public ActionResult DashboardSoporte()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Mensaje = "Bienvenido al panel de Soporte.";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [Authorize]
        public ActionResult DashboardAnalista()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Mensaje = "Bienvenido al panel de Analista.";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }


    }
}