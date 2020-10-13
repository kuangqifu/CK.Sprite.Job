using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CK.Sprite.JobWebHost.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("swagger");
        }
    }
}
