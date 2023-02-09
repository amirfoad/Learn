using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Learn.DataLayer.Entities.User;

namespace Learn.Core.DTOs
{
    public class UsersForAdminViewModel
    {
        public List<User> users { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
      

    }
    public class CreateUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [Remote("VerifyEmail", "Account")]
        public string Email { get; set; }


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

        [Display(Name = "توضیح")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Description { get; set; }
        public IFormFile UserAvatar { get; set; }
      

    }
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]

        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        //[Remote("VerifyEmail", "Account")]
        public string Email { get; set; }

       [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

   
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه های عبور تطابق ندارند.")]
        public string RePassword { get; set; }
        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }
        [Display(Name = "توضیح")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Description { get; set; }
        public List<int> UserRoles { get; set; }


    }
}
