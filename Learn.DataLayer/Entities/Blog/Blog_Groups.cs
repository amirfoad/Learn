using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Blog
{
    public class Blog_Groups
    {
        [Key]
        public int GroupID { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string GroupTitle { get; set; }
        [Display(Name = "گروه مادر")]
        public int? ParentID { get; set; }
        public bool IsDelete { get; set; }


        #region Relations


        [ForeignKey("ParentID")]
        public List<Blog_Groups> blog_Groups { get; set; }
        [InverseProperty("Group")]

        public List<Blog> Products { get; set; }

        [InverseProperty("ProductGroup")]

        public List<Blog> SubGroup { get; set; }

        #endregion
    }
}
