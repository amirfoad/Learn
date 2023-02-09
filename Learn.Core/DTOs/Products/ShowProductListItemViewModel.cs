 using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.DTOs.Products
{
    public class ShowProductListItemViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Altname { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public string ParentGroup { get; set; }
        public string ManbaName { get; set; }
        public string ManbaLink { get; set; }
        public string ManbaImageName { get; set; }

        public TimeSpan? TotalTime { get; set; }

    }
}
