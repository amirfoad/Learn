using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Learn.DataLayer.Entities.User;

namespace Learn.DataLayer.Entities.Orders
{
    public class Order
    {
        public Order()
        {

        }
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }

        public bool IsFinally { get; set; }
        [Required]

        public DateTime CreateDate { get; set; }
        [Required]

        public int OrderSum { get; set; }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual List<OrderDetail> OrderDetail { get; set; }

        #endregion



    }
}
