namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Message : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
