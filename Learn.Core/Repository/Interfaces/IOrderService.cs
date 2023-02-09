using System;
using System.Collections.Generic;
using System.Text;
using Learn.Core.DTOs.Order;
using Learn.DataLayer.Entities.Orders;

namespace Learn.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(string userName,int productId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName,int orderId);
        Order GetOrderById(int orderId);
        void UpdateOrder(Order order);
        bool FinalyOrder(string userName,int orderId);
        List<Order> GetUserOrders(string userName);
        bool IsUserInProduct(string userName, int productId);

        #region Discount

        DiscountUseType UseDiscount(int orderId, string code);
        void AddDiscount(Discount discount);
        List<Discount> GetAllDiscounts();
        Discount GetDiscountById(int id);
        void UpdateDiscount(Discount discount);
        bool IsExistCode(string code);
        #endregion
    }
}
