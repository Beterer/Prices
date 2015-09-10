using Prices.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prices.Controllers
{
    public class PricesController : Controller
    {
        public ActionResult Index()
        {
			ViewBag.products = ServiceProxy.ConsumeService();
            return View();
        }

    }
}
