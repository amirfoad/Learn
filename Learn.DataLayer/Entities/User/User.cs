using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Learn.DataLayer.Entities.Products;

namespace Learn.DataLayer.Entities.User
{
    public class User
    {
        public User()
        {

        }


        [Key]
        public int UserId { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }


        [Display(Name = "کدفعال سازی")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]

        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]

        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        public bool IsDelete { get; set; }
        [Display(Name = "توضیح")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Description { get; set; }

        [Display(Name = "لینکداین")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Linkedin { get; set; }

        [Display(Name = "توییتر")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Twitter { get; set; }

        [Display(Name = "فیسبوک")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string FaceBook { get; set; }



        #region Relations

        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Wallet.Wallet> Wallets { get; set; }
        public virtual List<Orders.Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public List<UserProduct> UserProducts { get; set; }

        public List<UserDiscountCode> UserDiscountCodes { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<ProductVote> ProductVotes { get; set; }


        #endregion

    }
}
