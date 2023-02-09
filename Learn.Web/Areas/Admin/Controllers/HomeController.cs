using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
