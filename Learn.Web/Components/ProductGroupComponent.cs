using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Services.Interfaces;

namespace Learn.Web.Components
{
    public class ProductGroupComponent :ViewComponent
    {
        private IProductService _productService;
        public ProductGroupComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("ProductGroup",_productService.GetAllGroups()));
        }
    }
}
