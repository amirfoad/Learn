using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Learn.Core.Services.Interfaces;
using Learn.Core.Repository.Interfaces;

namespace Learn.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userservice;
        private IProductService _productService;
        private IPagesService _pagesService;
        public HomeController(IUserService userService, IProductService productService,IPagesService pagesService)
        {
            _userservice = userService;
            _productService = productService;
            _pagesService = pagesService;
        }
        public IActionResult Index()
        {
            var popular = _productService.GetPopularProducts(8);
            ViewBag.popularProduct = popular;
            ViewBag.Groups = _productService.GetSubGroup();
            return View(_productService.GetProduct().Item1);
        }



    }
}
