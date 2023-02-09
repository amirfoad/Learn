using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class ProductsController : Controller
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetProductFroAdmin());
        }

        #region CreateProducts

        public IActionResult Create()
        {
            //Goroohe asli
            var groups = _productService.GetGroupForManageProduct();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");
           
            var Manba = _productService.GetManbaForManageProduct();
            ViewData["Manba"] = new SelectList(Manba, "Value", "Text");

            //Sub Group
            var SubGroup = _productService.GetSubGroupForManageProduct(int.Parse(groups.First().Value));
            ViewData["SubGroup"] = new SelectList(SubGroup, "Value", "Text");


            //Teacher (Users With Rol Teacher)
            var teachers = _productService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

            // Levels
            var levels = _productService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");


            // Statues
            var stautes = _productService.GetSatues();
            ViewData["Statues"] = new SelectList(stautes, "Value", "Text");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
            {
                //Goroohe asli
                var groups = _productService.GetGroupForManageProduct();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text");
                var Manba = _productService.GetManbaForManageProduct();
                ViewData["Manba"] = new SelectList(Manba, "Value", "Text");
                //Sub Group
                var SubGroup = _productService.GetSubGroupForManageProduct(int.Parse(groups.First().Value));
                ViewData["SubGroup"] = new SelectList(SubGroup, "Value", "Text");


                //Teacher (Users With Rol Teacher)
                var teachers = _productService.GetTeachers();
                ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

                // Levels
                var levels = _productService.GetLevels();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text");


                // Statues
                var stautes = _productService.GetSatues();
                ViewData["Statues"] = new SelectList(stautes, "Value", "Text");
                return View(product);
            }


            _productService.AddProduct(product, imgCourseUp, demoUp);


            return Redirect("/Admin/Products");
        }


        #endregion 

        #region EditProducts

        public IActionResult Edit(int id)
        {

            Product product = _productService.GetProductById(id);
            //Goroohe asli
            var groups = _productService.GetGroupForManageProduct();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", product.GroupId);
            var Manba = _productService.GetManbaForManageProduct();
            ViewData["Manba"] = new SelectList(Manba, "Value", "Text",product.ManbaId);
            //Sub Group
            List<SelectListItem> subgroup = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کنید",
                    Value=""
                }
            };
            subgroup.AddRange(_productService.GetSubGroupForManageProduct(product.GroupId));
            string SelectedSubGroup = "";
            if (product.SubGroup != null)
            {
                SelectedSubGroup = product.SubGroup.ToString();
            }
            ViewData["SubGroup"] = new SelectList(subgroup, "Value", "Text", SelectedSubGroup);


            //Teacher (Users With Rol Teacher)
            var teachers = _productService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", product.TeacherId);

            // Levels
            var levels = _productService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text", product.LevelId);


            // Statues
            var stautes = _productService.GetSatues();
            ViewData["Statues"] = new SelectList(stautes, "Value", "Text", product.StatusId);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
            {
                var Manba = _productService.GetManbaForManageProduct();
                ViewData["Manba"] = new SelectList(Manba, "Value", "Text", product.ManbaId);
                //Goroohe asli
                var groups = _productService.GetGroupForManageProduct();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", product.GroupId);

                //Sub Group
                List<SelectListItem> subgroup = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کنید",
                    Value=""
                }
            };
                subgroup.AddRange(_productService.GetSubGroupForManageProduct(product.GroupId));
                string SelectedSubGroup = "";
                if (product.SubGroup != null)
                {
                    SelectedSubGroup = product.SubGroup.ToString();
                }
                ViewData["SubGroup"] = new SelectList(subgroup, "Value", "Text", SelectedSubGroup);


                //Teacher (Users With Rol Teacher)
                var teachers = _productService.GetTeachers();
                ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", product.TeacherId);

                // Levels
                var levels = _productService.GetLevels();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text", product.LevelId);


                // Statues
                var stautes = _productService.GetSatues();
                ViewData["Statues"] = new SelectList(stautes, "Value", "Text", product.StatusId);
                return View(product);
            }

            _productService.UpdateProduct(product, imgCourseUp, demoUp);
            return Redirect("/Admin/Products");
        }

        #endregion


        #region DeleteProducts

        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return Content("TRUE");
        }

        #endregion


        #region JqueryProducts

        public JsonResult GetSubGroups(int id)
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem()
                {

                    Text="انتخاب کنید",
                    Value=""
                }
            };
            items.AddRange(_productService.GetSubGroupForManageProduct(id));

            return Json(new SelectList(items, "Value", "Text"));
        }
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
        #endregion

        #region episode

    
   
        public IActionResult CreateEpisode(int id)
        {
          
            
            ViewData["seasonId"] = id;
            return View();
        }

        [HttpPost]

        public IActionResult CreateEpisode(ProductEpisode episode, IFormFile fileEpisode)
        {
            
            if (!ModelState.IsValid || fileEpisode == null)
            {
                

                return View(episode);

            }
            
             
            if (_productService.CheckExistFile(fileEpisode.FileName))
            {
           

                ViewData["IsExistFile"] = true;
                return View(episode);
            }

            _productService.AddEpisode(episode, fileEpisode);

            return Redirect("/Admin/Products/IndexSeason/" + episode.Season.ProductId);
        }
        public IActionResult EditEpisode(int id)
        {
            var episode = _productService.GetEpisodeById(id);
   
            return View(episode);
        }
        [HttpPost]
        public IActionResult EditEpisode(ProductEpisode episode,IFormFile fileEpisode)
        {
            if (!ModelState.IsValid)
            {
               
                return View(episode);
            }
              
            if (fileEpisode != null)
            {
                if (_productService.CheckExistFile(fileEpisode.FileName))
                {
                
                    ViewData["IsExistFile"] = true;
                    return View(episode);
                }
            }

            _productService.EditEpisode(episode, fileEpisode);

            return Redirect("/Admin/Products/Indexseason/" + episode.Season.ProductId);
        }

        public IActionResult DeleteEpisode(int id)
        {
            _productService.DeleteEpisode(id);
            return Ok();
        }
        #endregion

        #region Season
        public IActionResult IndexSeason(int id)
        {
            ViewData["ProductId"] = id;

            return View(_productService.GetListSeasons(id));
        }
        public IActionResult CreateSeason(int id)
        {
            ViewData["ProductId"] = id;
            return View();
        }
        [HttpPost]
        public IActionResult CreateSeason(Season season)
        {
            if (!ModelState.IsValid)
                return View(season);
            _productService.AddSeason(season);
            return Redirect("/Admin/Products/IndexSeason/" + season.ProductId);
        }
        public IActionResult EditSeason(int id)
        {
           
            return View(_productService.GetSeasonById(id));
        }
        [HttpPost]
        public IActionResult EditSeason(Season season)
        {
            if (!ModelState.IsValid)
                return View(season);
            _productService.UpdateSeason(season);
            return Redirect("/Admin/Products/IndexSeason/" + season.ProductId);
        }

        public IActionResult DeleteSeason(int id)
        {
            _productService.DeleteSeason(id);
            return Ok();
        }

        #endregion

        #region Manba

        public IActionResult IndexManba()
        {
            return View(_productService.GetAllManba());
        }

        public IActionResult CreateManba()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateManba(Manba manba,IFormFile imgManba)
        {
            if (!ModelState.IsValid)
                return View(manba);

            _productService.AddManba(manba, imgManba);

            return Redirect("/Admin/Products/IndexManba");
        }


        public IActionResult EditManba(int id)
        {
            return View(_productService.GetManbaById(id));
        }
        [HttpPost]
        public IActionResult EditManba(Manba manba, IFormFile imgManba)
        {
            if (!ModelState.IsValid)
                return View(manba);

            _productService.UpdateManba(manba, imgManba);

            return Redirect("/Admin/Products/IndexManba");
        }
        #endregion
    }
}
