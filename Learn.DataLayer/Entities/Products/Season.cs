using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
   public class Season
    {
        [Key]
        public int SeasonId { get; set; }
        [Display(Name = "عنوان بخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Title { get; set; }
        public int ProductId { get; set; }

        public bool IsDelete { get; set; }
        #region Relations

        public List<ProductEpisode> ProductEpisodes { get; set; }
        public Product Product { get; set; }


        #endregion
    }
}
