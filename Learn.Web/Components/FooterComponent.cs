using Learn.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Components
{
    public class FooterComponent : ViewComponent
    {
        private IPagesService _pagesService;
        public FooterComponent(IPagesService pagesService)
        {
            _pagesService = pagesService;
        }
   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("Footer",_pagesService.GetFooter()));
        }
    }
}
