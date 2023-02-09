using Learn.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Controllers
{
    public class BlogsController : Controller
    {
        private IBlogService _blogSerrvice;
        public BlogsController(IBlogService blogService)
        {
            _blogSerrvice = blogService;
        }
        public IActionResult Index(int pageId = 1, int take = 0, string filter = "", List<int> selectedGroups = null)
        {
            ViewBag.pageId = pageId;
            ViewBag.Groups = _blogSerrvice.GetAllGroups();
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.filter = filter;
            return View(_blogSerrvice.GetBlog(pageId,9,filter,selectedGroups));
        }
        [Route("ShowBlog/{id}")]
        public IActionResult ShowBlog(int id)
        {
            var post = _blogSerrvice.GetBlogById(id);
            if(post==null)
            {
                return NotFound();
            }
            ViewData["Groups"] = _blogSerrvice.GetAllGroups();
            ViewData["LatestPosts"] = _blogSerrvice.GetLatestBlog();
            return View(post);
        }
    }
}
