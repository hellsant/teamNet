using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestService.Controllers
{
    public class MockController : Controller
    {
        //
        // GET: /Mock/
        public ActionResult Index()
        {
            return View();
        }
	}
}