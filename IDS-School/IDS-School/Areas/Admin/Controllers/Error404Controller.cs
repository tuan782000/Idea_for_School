using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Error404Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
