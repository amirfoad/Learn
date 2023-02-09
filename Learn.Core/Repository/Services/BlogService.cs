using Learn.Core.Convertors;
using Learn.Core.DTOs.Blog;
using Learn.Core.Genarator;
using Learn.Core.Repository.Interfaces;
using Learn.Core.Security;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Learn.Core.Repository.Services
{
    public class BlogService : IBlogService
    {
        private LearnContext _context;
        public BlogService(LearnContext context)
        {
            _context = context;
        }

        public int AddBlog(Blog blog, IFormFile imageBlog)
        {
            blog.CreateDate = DateTime.Now;
            blog.ImageName = "images.jpg";

            //Save Picture
            if (imageBlog != null && imageBlog.IsImage())
            {
                blog.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imageBlog.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog", blog.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imageBlog.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Thumb", blog.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }

            _context.Blogs.Add(blog);
            _context.SaveChanges();


            _context.SaveChanges();

            return blog.BlogID;
        }

        public void AddGroup(Blog_Groups blog_Groups)
        {
            _context.Blog_Groups.Add(blog_Groups);
            _context.SaveChanges();
        }

        public void DeleteBlogById(int id)
        {
            var blog = GetBlogById(id);
            blog.IsDelete = true;
            _context.SaveChanges();
        }

        public void DeleteGroup(Blog_Groups blog_Groups)
        {
            blog_Groups.IsDelete = true;
            UpdateGroup(blog_Groups);

        }

        public void DeleteGroupById(int groupId)
        {
            var group = GetGroupById(groupId);
            group.IsDelete = true;
            _context.SaveChanges();
        }

        public List<Blog_Groups> GetAllGroups()
        {
            return _context.Blog_Groups
                .Include(b => b.blog_Groups)
                .ToList();
        }

        public Tuple<List<ShowBlogListItemViewModel>, int> GetBlog(int pageId = 1, int take = 0, string filter = "", List<int> selectedGroups = null)
        {
            IQueryable<Blog> result = _context.Blogs;

            if (take == 0)
                take = 8;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.Title.Contains(filter) || c.Tags.Contains(filter));
            }





            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int item in selectedGroups)
                {
                    result = result.Where(r => r.GroupId == item || r.SubGroup == item);
                }
            }

            int skip = (pageId - 1) * take;

            var res = result.Include(p => p.ProductGroup).Skip(skip).Take(take).ToList().Select(r => new ShowBlogListItemViewModel()
            {
                BlogId = r.BlogID,
                ImageName = r.ImageName,
                Title = r.Title,
                Altname = "",
                CreateDate = r.CreateDate,
                ShortDescription = r.ShortDescription,
                ParentGroup = r.ProductGroup.GroupTitle,


            });
            int pageCount = result.Include(p => p.ProductGroup).ToList().Select(r => new ShowBlogListItemViewModel()
            {
                BlogId = r.BlogID,
                ImageName = r.ImageName,
                Title = r.Title,
                CreateDate = r.CreateDate,
                ShortDescription = r.ShortDescription,

                Altname = "",
                ParentGroup = r.ProductGroup.GroupTitle,


            }).Count() / take;


            return Tuple.Create(res.ToList(), pageCount);
        }

        public Blog GetBlogById(int blogId)
        {
            return _context.Blogs.Find(blogId);
        }

        public List<ShowBlogForAdminViewModel> GetBlogForAdmin()
        {
            return _context.Blogs
                .Select(b => new ShowBlogForAdminViewModel()
                {
                    BlogId = b.BlogID,
                    BlogTitle = b.Title,
                    ImageName = b.ImageName
                }).ToList();
        }



        public Blog_Groups GetGroupById(int groupId)
        {
            return _context.Blog_Groups.Find(groupId);
        }

        public List<SelectListItem> GetGroupForManageBlog()
        {
            return _context.Blog_Groups
            .Where(g => g.ParentID == null)
            .Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupID.ToString()
            }).ToList();
        }

        public List<ShowLatestBlogViewModel> GetLatestBlog()
        {
            return _context.Blogs.OrderByDescending(b=>b.CreateDate).Take(3).Select(b => new ShowLatestBlogViewModel()
            {
                BlogId=b.BlogID,
                Title=b.Title,
                ImageName=b.ImageName
            })
                .ToList();
        }

        public List<SelectListItem> GetSubGroupForManageBlog(int groupId)
        {
            return _context.Blog_Groups
        .Where(g => g.ParentID == groupId)
        .Select(g => new SelectListItem()
        {
            Text = g.GroupTitle,
            Value = g.GroupID.ToString()
        }).ToList();
        }

        public int UpdateBlog(Blog blog, IFormFile imageBlog)
        {


            //Save Picture
            if (imageBlog != null && imageBlog.IsImage())
            {
                if (blog.ImageName != "images.jpg")
                {
                    string DeleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog", blog.ImageName);
                    if (File.Exists(DeleteImagePath))
                    {
                        File.Delete(DeleteImagePath);
                    }
                    string DeleteImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Thumb", blog.ImageName);
                    if (File.Exists(DeleteImagePathThumb))
                    {
                        File.Delete(DeleteImagePathThumb);
                    }
                }
                blog.ImageName = NameGenarator.GenarateUniqCode() + Path.GetExtension(imageBlog.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog", blog.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imageBlog.CopyTo(stream);
                }

                //TODO Image Resize
                string ThumPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Thumb", blog.ImageName);

                ImageConvertor imgResize = new ImageConvertor();
                imgResize.Image_resize(ImagePath, ThumPath, 150);
            }

            _context.Blogs.Update(blog);
            _context.SaveChanges();

            _context.SaveChanges();

            return blog.BlogID;
        }

        public void UpdateGroup(Blog_Groups blog_Groups)
        {

            _context.Blog_Groups.Update(blog_Groups);
            _context.SaveChanges();

        }
    }
}
