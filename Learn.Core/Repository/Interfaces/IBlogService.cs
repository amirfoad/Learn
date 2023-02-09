using Learn.Core.DTOs.Blog;
using Learn.DataLayer.Entities.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Repository.Interfaces
{
    public interface IBlogService
    {
        #region Group

        List<Blog_Groups> GetAllGroups();

        void AddGroup(Blog_Groups blog_Groups);
        void UpdateGroup(Blog_Groups blog_Groups);
        Blog_Groups GetGroupById(int groupId);
        List<SelectListItem> GetGroupForManageBlog();
        List<SelectListItem> GetSubGroupForManageBlog(int groupId);
        void DeleteGroup(Blog_Groups blog_Groups);
        void DeleteGroupById(int groupId);
        List<ShowLatestBlogViewModel> GetLatestBlog();
        Tuple<List<ShowBlogListItemViewModel>, int> GetBlog(int pageId = 1, int take = 0, string filter = "", List<int> selectedGroups = null);



        #endregion

        #region Blog

        List<ShowBlogForAdminViewModel> GetBlogForAdmin();
        void DeleteBlogById(int id);
        Blog GetBlogById(int blogId);
        int AddBlog(Blog blog, IFormFile imageBlog);
        int UpdateBlog(Blog blog, IFormFile imageBlog);

        #endregion
    }
}
