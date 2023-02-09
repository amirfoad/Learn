using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
   public  class ProductVote
    {
        [Key]
        public int VoteId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public bool Vote { get; set; }
        public DateTime VoteDate { get; set; } = DateTime.Now;


        #region Relations

        public User.User User { get; set; }
        public Product Product { get; set; }


        #endregion
    }
}
