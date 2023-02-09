using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Learn.Core.DTOs;
using Learn.Core.Services.Interfaces;

namespace Learn.Web.Areas.UserPanel.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.WalletList = _userService.GetWalletUser(User.Identity.Name);

            return View();
        }
        [HttpPost, Route("UserPanel/Wallet")]
        public IActionResult Index(ChargeWalletViewModel wallet)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.WalletList = _userService.GetWalletUser(User.Identity.Name);
                return View(wallet);
            }

            int walletId = _userService.ChargeWallet(User.Identity.Name, "شارژ حساب", wallet.Amount, false);


            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(wallet.Amount);

            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44335/OnlinePayment/" + walletId,"Info@Learn.Com", "09197070750");

            if(res.Result.Status==100)
            {
               return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion

            return null;
        }
    }
}
