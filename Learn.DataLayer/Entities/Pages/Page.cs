using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Pages
{
    public class Page
    {
        [Key]
        public int PageID { get; set; }
        [Display(Name = "کد شناسایی")]
        public Nullable<int> PageIdentity { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Display(Name = "توضیح")]
        public string ShortDescription { get; set; }
        [Display(Name = "بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Body { get; set; }
    }
}
