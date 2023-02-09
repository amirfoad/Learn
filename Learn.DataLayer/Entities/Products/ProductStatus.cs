using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    //Vaziate Dore
    public class ProductStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Display(Name = "عنوان وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string StatusTitle { get; set; }


        #region Relations

        public virtual List<Product> Products { get; set; }


        #endregion

    }
}
