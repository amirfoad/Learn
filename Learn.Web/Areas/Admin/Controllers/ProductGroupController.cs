using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class ProductGroupController : Controller
    {
        private IProductService _productService;
        public ProductGroupController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetAllGroups());
        }
        #region Create

        public IActionResult Create(int? id)
        {
            return View(new ProductGroup()
            {
                ParentId = id
            });
        }
        [HttpPost]
        public IActionResult Create(ProductGroup Group)
        {
            if (!ModelState.IsValid)
                return View();

            _productService.AddGroup(Group);

            return Redirect("/admin/ProductGroup");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            return View(_productService.GetGroupById(id));
        }
        [HttpPost]
        public IActionResult Edit(ProductGroup Group)
        {

            if (!ModelState.IsValid)
                return View(Group);

            _productService.UpdateGroup(Group);

            return Redirect("/admin/ProductGroup");
        }

        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            _productService.DeleteGroupById(id);
            return Ok();
        }

        #endregion
    }
}
