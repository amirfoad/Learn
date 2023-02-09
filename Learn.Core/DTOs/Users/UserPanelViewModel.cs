using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.Core.DTOs
{
    public class InformationUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }

    }
    public class SideBarUserPanelViewModel
    {
        public string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ImageName { get; set; }


    }

    public class EditProfileViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Email { get; set; }
        
        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

    }
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور فعلی")]
        public string  OldPassword { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه های عبور تطابق ندارند.")]
        public string RePassword { get; set; }
    }
}
