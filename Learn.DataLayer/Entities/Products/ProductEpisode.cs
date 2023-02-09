using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Text;

namespace Learn.DataLayer.Entities.Products
{
    public class ProductEpisode
    {
        public ProductEpisode()
        {

        }

        [Key]
        public int EpisodeId { get; set; }


        public int? SeasonId { get; set; }


        [Display(Name = "عنوان بخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string EpisodeTitle { get; set; }


        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public TimeSpan EpisodeTime { get; set; }
        
        [Display(Name = "فایل")]
        public string EpisodeFileName { get; set; }

        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }

        public bool IsDelete { get; set; }



        #region Relations


        [ForeignKey("SeasonId")]
        public  Season Season { get; set; }

        #endregion

    }
}
