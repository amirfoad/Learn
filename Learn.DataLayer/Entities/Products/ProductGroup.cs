using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    public class ProductGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string GroupTitle { get; set; }
        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }
        [Display(Name = "گروه اصلی")]
        public int? ParentId { get; set; }

        #region Relations


        [ForeignKey("ParentId")]
        public List<ProductGroup> productGroup { get; set; }
        [InverseProperty("Group")]

        public List<Product> Products { get; set; }

        [InverseProperty("ProductGroup")]

        public List<Product> SubGroup { get; set; }



        #endregion

    }
}
