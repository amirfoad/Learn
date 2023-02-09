using Learn.Core.Repository.Interfaces;
using Learn.Core.Security;
using Learn.DataLayer.Entities.Pages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class PagesController : Controller
    {
        private IPagesService _pagesService;
        public PagesController(IPagesService pagesService)
        {
            _pagesService = pagesService;
        }
        public IActionResult Index()
        {
            return View(_pagesService.GetAllPages());
        }
        #region Create

        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Page page)
        {
            if (!ModelState.IsValid)
                return View(page);

            
            if (!_pagesService.CheckIdentityId(page.PageIdentity))
            {
                ModelState.AddModelError("PageIdentity", "کد شناسایی تکراری است.");
                return View(page);
            }

            _pagesService.AddPage(page);

            return Redirect("/Admin/Pages");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            return View(_pagesService.GetPageById(id));
        }
        [HttpPost]
        public IActionResult Edit(Page page)
        {
            if (!ModelState.IsValid)
                return View(page);
            if (!_pagesService.CheckIdentityId(page.PageIdentity))
            {
                ModelState.AddModelError("PageIdentity", "کد شناسایی تکراری است.");
                return View(page);
            }
            _pagesService.UpdatePage(page);

            return Redirect("/Admin/Pages");

        }
        #endregion

        #region Delete

        public IActionResult Delete(int id)
        {
            _pagesService.DeletePage(id);
            return Ok();
        }

        #endregion
    }
}
