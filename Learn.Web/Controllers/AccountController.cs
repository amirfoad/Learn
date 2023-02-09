using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Learn.Core.Convertors;
using Learn.Core.Genarator;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Learn.Core.Senders;

namespace Learn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }
        #region Register
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost, Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمیباشد.");
                return View(register);
            }

            if (_userService.IsExistEmail(FixedText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است.");
                return View(register);

            }

            //TODO : Register User
            Learn.DataLayer.Entities.User.User user = new DataLayer.Entities.User.User()
            {
                ActiveCode = NameGenarator.GenarateUniqCode(),
                Email = FixedText.FixEmail(register.Email),
                IsActive = true,
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                UserAvatar = "Defult.jpg",
                UserName = register.UserName
            };
            _userService.AddUser(user);

            #region Send Activation Email

            //string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            //SendEmail.Send(user.Email, "فعال سازی", body);

            #endregion
            //TODO: Login User

            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                         new Claim(ClaimTypes.Name, user.UserName)
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            HttpContext.SignInAsync(principal, properties);

            ViewBag.IsSuccess = true;

            return Redirect("/UserPanel");
        }
        public IActionResult VerifyEmail(string Email)
        {
            if (_userService.IsExistEmail(FixedText.FixEmail(Email)))
            {
                return Json($"ایمیل {Email}تکراری است.");
            }
            return Json(true);
        }
        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login(bool EditProfile=false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }
        [HttpPost, Route("Login")]
        public IActionResult Login(LoginViewModel login,string returnUrl="/")
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    //TODO: Login User

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                         new Claim(ClaimTypes.Name, user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;

                    if(returnUrl!="/")
                    {
                        return Redirect(returnUrl);
                    }

                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمیباشد.");
                    return View(login);
                }

            }
            ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده پیدا نشد.");
            return View(login);
        }
        #endregion

        #region ActiveAccount

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }

        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost, Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string email = FixedText.FixEmail(forgot.Email);
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد.");
                return View(forgot);
            }

            string BodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", BodyEmail);
            ViewBag.IsSuccess = true;

            return View();
        }
        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id

            });
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            var user = _userService.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
                return NotFound();


            string hassPass = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hassPass;
            _userService.UpateUser(user);

            return Redirect("/Login");
        }

        #endregion

        #region LogOut

        [Route("LogOut")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login");
        }

        #endregion

    }
}
