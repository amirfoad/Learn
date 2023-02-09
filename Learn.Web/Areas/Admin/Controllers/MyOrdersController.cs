using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Learn.Core.DTOs.Order;
using Learn.Core.Services.Interfaces;

namespace Learn.Web.Areas.UserPanel.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        private IOrderService _orderService;
        private IUserService _userService;
        public MyOrdersController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [Route("UserPanel/MyOrders")]
        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }
        [Route("UserPanel/MyOrders/ShowOrder/{id}")]
        public IActionResult ShowOrder(int id,bool finaly=false,string type="")
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);

            if (order == null)
                return NotFound();


            ViewBag.typeDiscount = type;
            ViewBag.finaly = finaly;


            return View(order);
        }
        [Route("UserPanel/MyOrders/FinalyOrder/{id}")]
        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinalyOrder(User.Identity.Name, id))
            {
                return Redirect("/UserPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }

            return BadRequest();
        }
        public IActionResult UseDiscount(int orderId,string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
          
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId + "?type=" + type.ToString());
        }
        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id, string Authority = "", string Status = "")
        {
            if (HttpContext.Request.Query["Status"] != "" &&
               HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
               HttpContext.Request.Query["Authority"] != "")
            {
                Authority = HttpContext.Request.Query["Authority"];

                var wallet = _userService.GetWalletByWalletId(id);

                var payment = new ZarinpalSandbox.Payment(wallet.Amount);

                var res = payment.Verification(Authority).Result;

                if (res.Status == 100)
                {
                    ViewBag.Code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay = true;
                    _userService.UpdateWallet(wallet);
                }


            }

            return View();
        }
    }
}
