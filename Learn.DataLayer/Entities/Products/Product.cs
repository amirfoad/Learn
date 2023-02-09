using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{


    public class Product
    {
        public Product()
        {

        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? SubGroup { get; set; }

        [Required]
        public int LevelId { get; set; }

        [Required]
        public int StatusId { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int TeacherId { get; set; }

        public int? ManbaId { get; set; }

        
        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string ProductTitle { get; set; }



        [Display(Name = "نام دوم محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Altername{ get; set; }



        [Display(Name = "شرح محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string ProductDescription { get; set; }


        [Display(Name = "شرح SEO")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string ProductSeoDescript { get; set; }

        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int ProductPrice { get; set; }

        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Tags { get; set; }

        [MaxLength(50)]
        public string ImageName { get; set; }

        [MaxLength(100)]
        public string DemoFileName { get; set; }


        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }


        public int? LanguageId { get; set; }

        #region Relations


        [ForeignKey("TeacherId")]
        public virtual User.User User { get; set; }


        [ForeignKey("GroupId")]
        public ProductGroup ProductGroup { get; set; }

        [ForeignKey("SubGroup")]
        public ProductGroup Group { get; set; }

        [ForeignKey("StatusId")]

        public virtual ProductStatus ProductStatus { get; set; }
        [ForeignKey("LevelId")]

        public virtual ProductLevel ProductLevel { get; set; }


        public virtual List<Season> Seasons { get; set; }
        public virtual List<Orders.OrderDetail> OrderDetails { get; set; }

        public List<UserProduct> UserProduct { get; set; }

        public List<ProductComment> ProductComments { get; set; }
        public List<ProductVote> ProductVotes { get; set; }

        public Manba Manba { get; set; }



        #endregion



    }
}
