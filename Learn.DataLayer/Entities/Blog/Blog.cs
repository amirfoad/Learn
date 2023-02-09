using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Blog
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]

        public string Title { get; set; }
        [Display(Name = "توضیح مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string ShortDescription { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? SubGroup { get; set; }
        [Display(Name = "بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Body { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "عکس")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string Tags { get; set; }


        #region Relations

        [ForeignKey("GroupId")]
        public Blog_Groups ProductGroup { get; set; }

        [ForeignKey("SubGroup")]
        public Blog_Groups Group { get; set; }


        #endregion
    }
}
