using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    //Sathe Dore (Course)

    public class ProductLevel
    {
        [Key]
        public int LevelId { get; set; }

        [Display(Name = "لول دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string LevelTitle { get; set; }


        #region Relations

        public virtual List<Product> Products { get; set; }


        #endregion


    }
}
