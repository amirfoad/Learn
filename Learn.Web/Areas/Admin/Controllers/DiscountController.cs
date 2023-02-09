using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Orders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]

    public class DiscountController : Controller
    {
        private IOrderService _orderService;
        public DiscountController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(_orderService.GetAllDiscounts());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Discount Discount ,string stDate = "", string edDate = "")
        {

            if (stDate != "")
            {
                string[] std = stDate.Split('/');
                Discount.StartDate = new DateTime(int.Parse(std[0]),
                    int.Parse(std[1]),
                    int.Parse(std[2]),
                    new PersianCalendar()
                    );
            }
            if (edDate != "")
            {

                string[] edd = edDate.Split('/');
                Discount.EndDate = new DateTime(int.Parse(edd[0]),
                    int.Parse(edd[1]),
                    int.Parse(edd[2]),
                    new PersianCalendar()
                    );
            }


            if (!ModelState.IsValid && _orderService.IsExistCode(Discount.DiscountCode))
                return View();


            _orderService.AddDiscount(Discount);

            return RedirectToAction("Index");
        
        }
        public IActionResult Edit(int id)
        {
            return View(_orderService.GetDiscountById(id));
        }
        [HttpPost]
        public IActionResult Edit(Discount Discount, string stDate = "", string edDate = "")
        {

            if (stDate != "")
            {
                string[] std = stDate.Split('/');
                Discount.StartDate = new DateTime(int.Parse(std[0]),
                    int.Parse(std[1]),
                    int.Parse(std[2]),
                    new PersianCalendar()
                    );
            }
            if (edDate != "")
            {

                string[] edd = edDate.Split('/');
                Discount.EndDate = new DateTime(int.Parse(edd[0]),
                    int.Parse(edd[1]),
                    int.Parse(edd[2]),
                    new PersianCalendar()
                    );
            }


            if (!ModelState.IsValid)
                return View();


            _orderService.UpdateDiscount(Discount);

            return RedirectToAction("Index");
        }
        public IActionResult CheckCode(string code)
        {
            return Content(_orderService.IsExistCode(code).ToString());
        }
    }
}
