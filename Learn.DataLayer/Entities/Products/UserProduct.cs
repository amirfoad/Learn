using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    public class UserProduct
    {
        [Key]
        public int UPId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }


        #region Relations

        public Product Product { get; set; }
        public User.User User { get; set; }


        #endregion
    }
}
