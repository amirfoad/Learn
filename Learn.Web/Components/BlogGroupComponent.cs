using Learn.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Components
{
    public class BlogGroupComponent : ViewComponent
    {
        private IBlogService _blogService;
        public BlogGroupComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("BlogGroup", _blogService.GetAllGroups()));
        }
    }
}
