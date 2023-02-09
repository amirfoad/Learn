using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.DTOs.Blog
{
    public class ShowBlogListItemViewModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Altname { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public string ShortDescription { get; set; }
        public string ParentGroup { get; set; }

    }
}
