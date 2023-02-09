using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Components
{
    public class UserMobileComponent:ViewComponent
    {
        private IUserService _userService;
        public UserMobileComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return await Task.FromResult((IViewComponentResult)View("UserMobile", _userService.GetUserByUserName(User.Identity.Name)));

        }
    }
}
