using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Learn.DataLayer.Entities.User;

namespace Learn.DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {

        }
        [Key]
        public int WalletId { get; set; }
        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
   
        public int TypeId{ get; set; }
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int UserId { get; set; }
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Amount { get; set; }
        [Display(Name ="تایید شده")]
        public bool IsPay { get; set; }
        [Display(Name = "شرح")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Description { get; set; }
        [Display(Name ="تاریخ وساعت")]
        public DateTime CreateDate { get; set; }


        #region Relations

        public virtual User.User User { get; set; }
        [ForeignKey("TypeId")]
        public virtual WalletType WalletType { get; set; }


        #endregion



    }
}
