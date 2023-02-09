using Learn.Core.Repository.Interfaces;
using Learn.Core.Security;
using Learn.DataLayer.Entities.Blog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class BlogGroupController : Controller
    {
        private IBlogService _blogService;
        public BlogGroupController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            return View(_blogService.GetAllGroups());
        }
        public IActionResult LisGroups()
        {
            return PartialView(_blogService.GetAllGroups());
        }
        #region Create

        public IActionResult Create(int? id)
        {
            return View(new Blog_Groups()
            {
                ParentID = id
            });
        }
        [HttpPost]
        public IActionResult Create(Blog_Groups group)
        {
            if (!ModelState.IsValid)
                return View(group);

            _blogService.AddGroup(group);

            return Redirect("/admin/BlogGroup");
        }
        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            return View(_blogService.GetGroupById(id));
        }
        [HttpPost]
        public IActionResult Edit(Blog_Groups group)
        {
            if (!ModelState.IsValid)
                return View(group);
            _blogService.UpdateGroup(group);
            return Redirect("/admin/BlogGroup");

        }
        #endregion

        #region Delete

        public IActionResult Delete(int id)
        {
            _blogService.DeleteGroupById(id);
            return Ok();
        }

        #endregion

    }
}
