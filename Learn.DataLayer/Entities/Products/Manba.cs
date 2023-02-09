using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    public class Manba
    {
        [Key]
        public int ManbaId { get; set; }
        [Display(Name = "نام منبع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Name { get; set; }
        [Display(Name = "توضیح اجمالی منبع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Description { get; set; }
        [MaxLength(50)]
        public string ImageName { get; set; }
        [MaxLength(500)]
        public string Link { get; set; }



        #region relations

        public List<Product> Products { get; set; }

        #endregion


    }
}
