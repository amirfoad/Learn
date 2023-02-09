using Learn.Core.DTOs.Footer;
using Learn.Core.DTOs.Pages;
using Learn.DataLayer.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Repository.Interfaces
{
    public interface IPagesService
    {
        #region Pages

       List<PageIndexAdminViewModel> GetAllPages();
        void AddPage(Page page);
        void UpdatePage(Page page);
        void DeletePage(int pageId);
        Page GetPageById(int pageId);
        Page GetPageByIdentifierId(int identifierId);
        //Page GetPageByTitle(string pageTitle);

        bool CheckIdentityId(int? identifierId);


        #endregion

        #region Footer

        int ProductsCount();
        int UsersCount();
        int TeachersCount();
        FooterViewModel GetFooter();

        #endregion
    }
}
