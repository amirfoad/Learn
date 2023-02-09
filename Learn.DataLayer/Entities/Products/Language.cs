using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }

        [Display(Name = "زبان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string LanguageName { get; set; }

        [Display(Name = "پرچم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(250, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Flag { get; set; }
    }
}
