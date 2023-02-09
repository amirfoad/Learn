using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Learn.Core.DTOs.Products;
using Learn.DataLayer.Entities.Products;
using Learn.Core.DTOs.Group;

namespace Learn.Core.Services.Interfaces
{
    public interface IProductService
    {
        #region Group

        List<ProductGroup> GetAllGroups();
        List<SelectListItem> GetGroupForManageProduct();
        List<SelectListItem> GetSubGroupForManageProduct(int groupId);
        List<SelectListItem> GetTeachers();

        List<GroupViewModel> GetSubGroup();

        List<SelectListItem> GetLevels();

        List<SelectListItem> GetSatues();
        void AddGroup(ProductGroup productGroup);
        void UpdateGroup(ProductGroup productGroup);
        ProductGroup GetGroupById(int groupId);

        void DeleteGroup(ProductGroup productGroup);
        void DeleteGroupById(int groupId);

        #endregion
        
        #region Product

        List<ShowProductForAdminViewModel> GetProductFroAdmin();

        int AddProduct(Product product, IFormFile imgProduct, IFormFile demo);

        Product GetProductById(int productId);
        void UpdateProduct(Product product, IFormFile imgProduct, IFormFile demo);
        Tuple<List<ShowProductListItemViewModel>, int> GetProduct(int pageId = 1, int take = 0, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null);
        Product GetProductForShowById(int productId);
        List<ShowProductListItemViewModel> GetPopularProducts(int take);

        bool IsFree(int productId);
        void DeleteProduct(Product product);
        void DeleteProduct(int productId);


        #endregion

        #region Manba

        List<SelectListItem> GetManbaForManageProduct();

        void AddManba(Manba manba, IFormFile imgManba);
        void UpdateManba(Manba manba, IFormFile imgManba);
        Manba GetManbaById(int manbaId);
        List<Manba> GetAllManba();

        #endregion

        #region Seasons

        List<Season> GetListSeasons(int productId);
        int AddSeason(Season season);
        void UpdateSeason(Season season);
        Season GetSeasonById(int seasonId);
        void DeleteSeason(int seasonId);

        #endregion

        #region Episode

        int AddEpisode(ProductEpisode episode, IFormFile file);
        bool CheckExistFile(string fileName);
        List<ProductEpisode> GetListEpisode(int productId);
        ProductEpisode GetEpisodeById(int episodeId);
        void EditEpisode(ProductEpisode episode, IFormFile file);
        List<SelectListItem> GetSeasonsForManageEpisode();
        void DeleteEpisode(int episodeId);


        #endregion

        #region Comments

        void AddComment(ProductComment productComment);
        Tuple<List<ProductComment>, int> GetProductComments(int productId, int pageId = 1);


        #endregion

        #region ProductVotes


        void AddVote(int userId, int productId, bool vote);
        Tuple<int, int> GetProductVotes(int productId);

        #endregion
    }
}
