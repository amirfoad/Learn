using Learn.Core.Repository.Interfaces;
using Learn.Core.Security;
using Learn.DataLayer.Entities.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class BlogController : Controller
    {
        private IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            return View(_blogService.GetBlogForAdmin());
        }

        #region Create

        public IActionResult Create()
        {
            //Goroohe asli
            var groups = _blogService.GetGroupForManageBlog();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            //Sub Group
            var SubGroup = _blogService.GetSubGroupForManageBlog(int.Parse(groups.First().Value));
            ViewData["SubGroup"] = new SelectList(SubGroup, "Value", "Text");

            return View();
        }
        [HttpPost]
        public IActionResult Create(Blog blog, IFormFile imageBlog)
        {
            if (!ModelState.IsValid)
            {

                //Goroohe asli
                var groups = _blogService.GetGroupForManageBlog();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text");

                //Sub Group
                var SubGroup = _blogService.GetSubGroupForManageBlog(int.Parse(groups.First().Value));
                ViewData["SubGroup"] = new SelectList(SubGroup, "Value", "Text");
                return View(blog);
            }

            _blogService.AddBlog(blog, imageBlog);

            return Redirect("/Admin/Blog");
        }
        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            var blog = _blogService.GetBlogById(id);
            //Goroohe asli
            var groups = _blogService.GetGroupForManageBlog();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", blog.GroupId);

            //Sub Group
            List<SelectListItem> subgroup = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کنید",
                    Value=""
                }
            };
            subgroup.AddRange(_blogService.GetSubGroupForManageBlog(blog.GroupId));
            string SelectedSubGroup = "";
            if (blog.SubGroup != null)
            {
                SelectedSubGroup = blog.SubGroup.ToString();
            }
            ViewData["SubGroup"] = new SelectList(subgroup, "Value", "Text", SelectedSubGroup);

            return View(blog);
        }
        [HttpPost]
        public IActionResult Edit(Blog blog, List<int> selectedGroups, IFormFile imageBlog)
        {
            if (!ModelState.IsValid)
            {
                //Goroohe asli
                var groups = _blogService.GetGroupForManageBlog();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", blog.GroupId);

                //Sub Group
                List<SelectListItem> subgroup = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کنید",
                    Value=""
                }
            };
                subgroup.AddRange(_blogService.GetSubGroupForManageBlog(blog.GroupId));
                string SelectedSubGroup = "";
                if (blog.SubGroup != null)
                {
                    SelectedSubGroup = blog.SubGroup.ToString();
                }
                ViewData["SubGroup"] = new SelectList(subgroup, "Value", "Text", SelectedSubGroup);
                return View(blog);
            }
            _blogService.UpdateBlog(blog, imageBlog);
            return Redirect("/Admin/Blog");
        }

        #endregion

        #region Jquery

        public JsonResult GetSubGroups(int id)
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem()
                {

                    Text="انتخاب کنید",
                    Value=""
                }
            };
            items.AddRange(_blogService.GetSubGroupForManageBlog(id));

            return Json(new SelectList(items, "Value", "Text"));
        }

        #endregion

        #region Delete

        public IActionResult Delete(int id)
        {
            _blogService.DeleteBlogById(id);
            return Ok();
        }


        #endregion
    }
}
