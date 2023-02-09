using Learn.Core.DTOs;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserPanelController : Controller
    {
        private IUserService _userService;

        public UserPanelController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("UserPanel")]
        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {

            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }
        [HttpPost, Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel edit)
        {
            if (!ModelState.IsValid)
                return View(edit);

            _userService.EditProfile(User.Identity.Name, edit);

            //Log Out

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return Redirect("/Login?EditProfile=true");
        }
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost, Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string CurrentUserName = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(changePassword);

            if (!_userService.CompareOldPassword(changePassword.OldPassword, CurrentUserName))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمیباشد.");
                return View(changePassword);

            }
            _userService.ChangeUserPassword(CurrentUserName, changePassword.Password);
            ViewBag.IsSuccess = true;
            return View();
        }
    }
}
