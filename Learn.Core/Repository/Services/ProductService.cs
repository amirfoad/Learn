using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Learn.Core.Convertors;
using Learn.Core.DTOs.Products;
using Learn.Core.Genarator;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Products;
using Learn.Core.DTOs.Group;

namespace Learn.Core.Services
{
    public class ProductService : IProductService
    {
        private LearnContext _context;
        public ProductService(LearnContext context)
        {
            _context = context;
        }

        public void AddComment(ProductComment productComment)
        {
            _context.ProductComments.Add(productComment);
            _context.SaveChanges();
        }

        public int AddEpisode(ProductEpisode episode, IFormFile file)
        {

            episode.EpisodeFileName = file.FileName;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", episode.EpisodeFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            _context.ProductEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;

        }

        public void AddGroup(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
        }

        public int AddProduct(Product product, IFormFile imgProduct, IFormFile demo)
        {
            product.CreateDate = DateTime.Now;
            product.ImageName = "no-photo.jpg";

            //Save Picture
            //TODO Check Image
            if (imgProduct != null && imgProduct.IsImage())
            {
                product.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imgProduct.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Images", product.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgProduct.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Thumb", product.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }
            if (demo != null)
            {
                product.DemoFileName = NameGenarator.GenarateUniqCode() + Path.GetExtension(demo.FileName);
                string DemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Demoes", product.DemoFileName);
                using (var stream = new FileStream(DemoPath, FileMode.Create))
                {
                    demo.CopyTo(stream);
                }

            }

            //TODO Upload Demo

            _context.Products.Add(product);
            _context.SaveChanges();
            return product.ProductId;

        }

        public int AddSeason(Season season)
        {
            _context.Seasons.Add(season);
            _context.SaveChanges();
            return season.SeasonId;
        }

        public void AddVote(int userId, int productId, bool vote)
        {
            var UserVote = _context.ProductVotes.FirstOrDefault(v => v.UserId == userId && v.ProductId == productId);
            if (UserVote != null)
            {
                UserVote.Vote = vote;
            }
            else
            {
                UserVote = new ProductVote()
                {
                    UserId = userId,
                    ProductId = productId,
                    Vote = vote
                };
                _context.ProductVotes.Add(UserVote);
            }
            _context.SaveChanges();

        }

        public bool CheckExistFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", fileName);
            return File.Exists(filePath);
        }

        public void DeleteGroup(ProductGroup productGroup)
        {
            productGroup.IsDelete = true;
            _context.SaveChanges();
        }

        public void DeleteGroupById(int groupId)
        {
            var group = _context.ProductGroups.Find(groupId);
            group.IsDelete = true;
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            product.IsDelete = true;
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            product.IsDelete = true;
            //_context.Remove(product);
            _context.SaveChanges();
        }

        public void EditEpisode(ProductEpisode episode, IFormFile file)
        {
            if (file != null)
            {
                string DeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", episode.EpisodeFileName);
                File.Delete(DeletePath);

                episode.EpisodeFileName = file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/ProductFiles", episode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            _context.ProductEpisodes.Update(episode);
            _context.SaveChanges();
        }

        public void UpdateSeason(Season season)
        {
            _context.Seasons.Update(season);
            _context.SaveChanges();
        }

        public List<ProductGroup> GetAllGroups()
        {
            return _context.ProductGroups
               
                .Include(p => p.productGroup).ToList();
        }

        public ProductEpisode GetEpisodeById(int episodeId)
        {
            return _context.ProductEpisodes
                .Include(e=>e.Season)
                .ThenInclude(s=>s.Product)
                .SingleOrDefault(e=>e.EpisodeId==episodeId);
        }

        public ProductGroup GetGroupById(int groupId)
        {
            return _context.ProductGroups.Find(groupId);
        }

        public List<SelectListItem> GetGroupForManageProduct()
        {
            return _context.ProductGroups
                .Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.ProductLevels
                .Select(l => new SelectListItem()
                {

                    Text = l.LevelTitle,
                    Value = l.LevelId.ToString()
                }).ToList();
        }

        public List<ProductEpisode> GetListEpisode(int productId)
        {
            return _context.ProductEpisodes.Where(p => p.SeasonId == productId).ToList();
        }

        public List<Season> GetListSeasons(int productId)
        {
            return _context.Seasons
                .Include(p=>p.ProductEpisodes)
                .Where(p => p.ProductId == productId)
                .ToList();
        }

        public List<ShowProductListItemViewModel> GetPopularProducts(int take = 8)
        {
            var res = _context.Products.Include(p => p.OrderDetails)
                .Include(p => p.Seasons)
                .ThenInclude(p => p.ProductEpisodes)
                .Include(p=>p.Manba)
                .Where(od => od.OrderDetails.Any())
                .OrderByDescending(od => od.OrderDetails.Count)
                .Take(take)
               .ToList()
                .Select(p => new ShowProductListItemViewModel()
                {
                    ProductId = p.ProductId,
                    Title = p.ProductTitle,
                    Altname = p.Altername,

                    ImageName = p.ImageName,
                    Price = p.ProductPrice,
                    ManbaName = p.Manba.Name,
                    ManbaImageName = p.Manba.ImageName,
                    ManbaLink = p.Manba.Link
                    //TotalTime = /*new TimeSpan()*/ /*new TimeSpan(p.ProductEpisodes.Sum(p => p.EpisodeTime.Ticks))*/

                });
            return res.ToList();

        }

        public Tuple<List<ShowProductListItemViewModel>, int> GetProduct(int pageId = 1, int take = 0, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            IQueryable<Product> result = _context.Products;

            if (take == 0)
                take = 8;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.ProductTitle.Contains(filter) || c.Tags.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    break;
                case "buy":
                    result = result.Where(p => p.ProductPrice != 0);
                    break;
                case "free":
                    result = result.Where(p => p.ProductPrice == 0);
                    break;
            }

            switch (orderByType)
            {
                case "date":

                    result = result.OrderByDescending(p => p.CreateDate);
                    break;


                case "updateDate":

                    result = result.OrderByDescending(p => p.UpdateDate);
                    break;


            }

            if (startPrice > 0)
            {
                result = result.Where(p => p.ProductPrice > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(p => p.ProductPrice < startPrice);
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int item in selectedGroups)
                {
                    result = result.Where(r => r.GroupId == item || r.SubGroup == item);
                }
            }

            int skip = (pageId - 1) * take;

            var res = result
                .Include(p => p.ProductGroup)
                .Include(p => p.Seasons)
                .ThenInclude(p=>p.ProductEpisodes)
                .Include(p=>p.Manba)
                .Skip(skip).Take(take).ToList().Select(r => new ShowProductListItemViewModel()
                {
                    ProductId = r.ProductId,
                    ImageName = r.ImageName,
                    Title = r.ProductTitle,
                    Altname = r.Altername,
                    Price = r.ProductPrice,
                    ParentGroup = r.ProductGroup.GroupTitle,
                    ManbaName=r.Manba.Name,
                    ManbaImageName=r.Manba.ImageName,
                    ManbaLink=r.Manba.Link
                    //TotalTime =new TimeSpan() /*new TimeSpan(r.ProductEpisodes.Sum(p => p.EpisodeTime.Ticks))*/

                });
            int pageCount = result
                .Include(p => p.ProductGroup)
                .Include(p => p.Seasons)
                .ThenInclude(p => p.ProductEpisodes)
               .ToList().Select(r => new ShowProductListItemViewModel()
               {
                   ProductId = r.ProductId,
                   ImageName = r.ImageName,
                   Title = r.ProductTitle,
                   Altname = r.Altername,
                   Price = r.ProductPrice,
                   ParentGroup = r.ProductGroup.GroupTitle,
                   ManbaName = r.Manba.Name,
                   ManbaImageName = r.Manba.ImageName,
                   ManbaLink = r.Manba.Link,
                   //TotalTime = new TimeSpan(r.Seasons)

               }).Count() / take;


            return Tuple.Create(res.ToList(), pageCount);

        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public Tuple<List<ProductComment>, int> GetProductComments(int productId, int pageId = 1)
        {

            int take = 5;
            int skip = (pageId - 1) * take;
            int pageCount = _context.ProductComments.Where(p => p.ProductId == productId).Count() / take;
            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }
            return Tuple.Create(_context.ProductComments.Include(p => p.User).Where(p => p.ProductId == productId).Skip(skip).Take(take).OrderByDescending(p => p.CreateDate).ToList(), pageCount);
        }

        public Product GetProductForShowById(int productId)
        {
            return _context.Products
                .Include(p => p.Seasons)
                .ThenInclude(p => p.ProductEpisodes)
                .Include(p => p.ProductGroup)
                .Include(p => p.ProductLevel)
                .Include(p => p.ProductStatus)
                .Include(p => p.ProductVotes)
                .Include(u => u.UserProduct)
                .Include(p => p.User)
                .Include(p => p.ProductComments)
                .FirstOrDefault(p => p.ProductId == productId);
        }

        public List<ShowProductForAdminViewModel> GetProductFroAdmin()
        {
            return _context.Products
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new ShowProductForAdminViewModel()
                {
                    ProductId = p.ProductId,
                    ImageName = p.ImageName,
                    Title = p.ProductTitle,
                    EpisodeCount = p.Seasons.Count

                }).ToList();
        }

        public Tuple<int, int> GetProductVotes(int productId)
        {
            var Votes = _context.ProductVotes.Where(p => p.ProductId == productId)
                .Select(v => v.Vote)
                .ToList();
            return Tuple.Create(Votes.Count(v => v == true), Votes.Count(v => v == false));
        }

        public List<SelectListItem> GetSatues()
        {
            return _context.ProductStatuses
               .Select(l => new SelectListItem()
               {

                   Text = l.StatusTitle,
                   Value = l.StatusId.ToString()
               }).ToList();
        }

        public Season GetSeasonById(int seasonId)
        {
            return _context.Seasons
                .Include(s=>s.ProductEpisodes)
                .Single(s=>s.SeasonId== seasonId);
        }

        public List<SelectListItem> GetSeasonsForManageEpisode()
        {
            return _context.Seasons
                .Select(s => new SelectListItem()
                {
                    Text = s.Title,
                    Value = s.SeasonId.ToString()
                }).ToList();
        }

        public List<GroupViewModel> GetSubGroup()
        {
            return _context.ProductGroups
                .Where(p => p.ParentId == null)
                .Select(p => new GroupViewModel()
            {
                GroupId = p.GroupId,
                GroupTitle = p.GroupTitle
            }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageProduct(int groupId)
        {
            return _context.ProductGroups
           .Where(g => g.ParentId == groupId)
           .Select(g => new SelectListItem()
           {
               Text = g.GroupTitle,
               Value = g.GroupId.ToString()
           }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles.Where(u => u.RoleId == 2)
                .Include(r => r.User)
                .Select(r => new SelectListItem()
                {
                    Text = r.User.UserName,
                    Value = r.UserId.ToString()
                }).ToList();
        }

        public bool IsFree(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId).Select(p => p.ProductPrice).First() == 0;
        }

        public void UpdateGroup(ProductGroup productGroup)
        {
            _context.ProductGroups.Update(productGroup);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product, IFormFile imgProduct, IFormFile demo)
        {
            product.UpdateDate = DateTime.Now;
            if (imgProduct != null && imgProduct.IsImage())
            {
                if (product.ImageName != "no-photo.jpg")
                {
                    string DeleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Images", product.ImageName);
                    if (File.Exists(DeleteImagePath))
                    {
                        File.Delete(DeleteImagePath);
                    }
                    string DeleteImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Thumb", product.ImageName);
                    if (File.Exists(DeleteImagePathThumb))
                    {
                        File.Delete(DeleteImagePathThumb);
                    }
                }
                product.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imgProduct.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Images", product.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgProduct.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Thumb", product.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }
            if (demo != null)
            {
                if (product.DemoFileName != null)
                {
                    string DeleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Demoes", product.DemoFileName);
                    if (File.Exists(DeleteDemoPath))
                    {
                        File.Delete(DeleteDemoPath);
                    }
                }
                product.DemoFileName = NameGenarator.GenarateUniqCode() + Path.GetExtension(demo.FileName);
                string DemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Demoes", product.DemoFileName);
                using (var stream = new FileStream(DemoPath, FileMode.Create))
                {
                    demo.CopyTo(stream);
                }

            }
            _context.Products.Update(product);
            _context.SaveChanges();

        }

        public void DeleteSeason(int seasonId)
        {
            var season = GetSeasonById(seasonId);
            if(season.ProductEpisodes.Any())
            {
                foreach (var episode in season.ProductEpisodes)
                {
                    episode.IsDelete = true;
                }
            }
            season.IsDelete = true;
            _context.SaveChanges();
               
        }

        public void DeleteEpisode(int episodeId)
        {
            var episode= GetEpisodeById(episodeId);
            episode.IsDelete = true;
            _context.SaveChanges();
        }

        public List<SelectListItem> GetManbaForManageProduct()
        {
            return _context.Manba
                 .Select(r => new SelectListItem()
                 {
                     Text = r.Name,
                     Value = r.ManbaId.ToString()
                 }).ToList();
        }

        public void AddManba(Manba manba, IFormFile imgManba)
        {
            manba.ImageName = "no-photo.jpg";

            //Save Picture
            //TODO Check Image
            if (imgManba != null && imgManba.IsImage())
            {
                manba.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imgManba.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba", manba.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgManba.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba/Thumb", manba.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }
            _context.Manba.Add(manba);
            _context.SaveChanges();
        }

        public void UpdateManba(Manba manba, IFormFile imgManba)
        {
            if (imgManba != null && imgManba.IsImage())
            {
                if (manba.ImageName != "no-photo.jpg")
                {
                    string DeleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba", manba.ImageName);
                    if (File.Exists(DeleteImagePath))
                    {
                        File.Delete(DeleteImagePath);
                    }
                    string DeleteImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba/Thumb", manba.ImageName);
                    if (File.Exists(DeleteImagePathThumb))
                    {
                        File.Delete(DeleteImagePathThumb);
                    }
                }
                manba.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imgManba.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba", manba.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgManba.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/Manba/Thumb", manba.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }
            _context.Manba.Update(manba);
            _context.SaveChanges();
        }

        public Manba GetManbaById(int manbaId)
        {
            return _context.Manba.Find(manbaId);
        }

        public List<Manba> GetAllManba()
        {
            return _context.Manba.ToList();
        }
    }
}
