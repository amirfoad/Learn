using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Products;

namespace Learn.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private IOrderService _orderService;
        private IUserService _userService;
        public ProductsController(IProductService productService,IOrderService orderService,IUserService userService)
        {
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
        }
        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.pageId = pageId;
            ViewBag.Groups = _productService.GetAllGroups();
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.getType = getType;
            ViewBag.filter = filter;
            return View(_productService.GetProduct(pageId,9,filter,getType,orderByType,startPrice,endPrice,selectedGroups));
        }
        [Route("ShowProduct/{id}")]
        public IActionResult ShowProduct(int id,int episodeId=0)
        {
            var product = _productService.GetProductForShowById(id);
            if(product==null)
            {
                return NotFound();
            }
            if(User.Identity.IsAuthenticated)
            {
              var user=   _userService.GetUserByUserName(User.Identity.Name);
                ViewData["UserImage"] = user.UserAvatar;
            }
            //if(episodeId!=0&&User.Identity.IsAuthenticated)
            //{

            //    if(!product.ProductEpisodes.All(p=>p.EpisodeId==episodeId))
            //    {
            //        return NotFound();
            //    }
            //    if(!product.ProductEpisodes.First(e=>e.EpisodeId==episodeId).IsFree)
            //    {
            //        if(!_orderService.IsUserInProduct(User.Identity.Name,id))
            //        {
            //            return NotFound();
            //        }
            //    }
            //    var episode = product.ProductEpisodes.First(e => e.EpisodeId == episodeId);
            //    ViewBag.episode = episode;
            //    string filePath = System.IO.Directory.GetCurrentDirectory();
            //    if (episode.IsFree)
            //    {
            //        filePath = System.IO.Path.Combine(filePath,"wwwroot/ProductOnline", episode.EpisodeFileName.Replace(".rar", ".mp4"));
            //    }
            //    else
            //    {
            //        filePath = System.IO.Path.Combine(filePath,"wwwroot/ProductFilesOnline", episode.EpisodeFileName.Replace(".rar", ".mp4"));

            //    }
            //    if(!System.IO.File.Exists(filePath))
            //    {
            //        string targetPath= System.IO.Directory.GetCurrentDirectory();
            //        if (episode.IsFree)
            //        {
            //            targetPath = System.IO.Path.Combine(targetPath,"wwwroot/ProductOnline");
            //        }
            //        else
            //        {
            //            targetPath = System.IO.Path.Combine(targetPath,"wwwroot/ProductFilesOnline");

            //        }
            //        string rarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", episode.EpisodeFileName);
            //        var archive = ArchiveFactory.Open(rarPath);
            //        var entries = archive.Entries.OrderBy(x => x.Key.Length);
            //        foreach (var item in entries)
            //        {
            //            if(Path.GetExtension(item.Key)==".mp4")
            //            {
            //                item.WriteTo(System.IO.File.Create(Path.Combine(targetPath, episode.EpisodeFileName.Replace(".rar", ".mp4"))));
            //            }
            //        }

            //    }
            //    ViewBag.filePath = filePath;
            //}
            var teacher= _userService.GetUserByUserId(product.TeacherId);
            ViewData["Teacher"] = teacher;
            return View(product);
        }

        [Authorize]
        public IActionResult BuyProducts(int id)
        {
           int orderId=  _orderService.AddOrder(User.Identity.Name, id);
            return Redirect("/userpanel/MyOrders/ShowOrder/" + orderId);
        }
           [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var Episode = _productService.GetEpisodeById(episodeId);
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", Episode.EpisodeFileName);
            string FileName = Episode.EpisodeFileName;
            if (Episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(FilePath);
                return File(file, "application/force-download",FileName);
            }
            if (User.Identity.IsAuthenticated)
            {
                if(_orderService.IsUserInProduct(User.Identity.Name,Episode.Season.ProductId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(FilePath);
                    return File(file, "application/force-download", FileName);
                }
            }

            return Forbid();
        }


        [HttpPost]
        public IActionResult CreateComment(ProductComment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.UserId = _userService.GetUserIdByUserName(User.Identity.Name);
            _productService.AddComment(comment);
            return View("ShowComment",_productService.GetProductComments(comment.ProductId));
        }

        public IActionResult ShowComment(int id,int pageId=1)
        {
            ViewBag.pageid = pageId;
            return View(_productService.GetProductComments(id, pageId));
        }
        public IActionResult ProductVote(int id)
        {
            if(!_productService.IsFree(id))
            {
                if (!_orderService.IsUserInProduct(User.Identity.Name, id))
                {
                    ViewBag.NotAccess = true;
                }
            }

            return PartialView(_productService.GetProductVotes(id));
        }
        [Authorize]
        public IActionResult AddVote(int id,bool vote)
        {
            _productService.AddVote(_userService.GetUserIdByUserName(User.Identity.Name), id, vote);
            return PartialView("ProductVote", _productService.GetProductVotes(id));
        }
    }
}
