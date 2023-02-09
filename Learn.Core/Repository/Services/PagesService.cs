using Learn.Core.DTOs.Footer;
using Learn.Core.DTOs.Pages;
using Learn.Core.Repository.Interfaces;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Core.Repository.Services
{
    public class PagesService : IPagesService
    {
        private LearnContext _context;
        private IProductService _productService;

        public PagesService(LearnContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public void AddPage(Page page)
        {
            _context.Pages.Add(page);
            _context.SaveChanges();
        }

        public bool CheckIdentityId(int? identifierId)
        {
            if(!_context.Pages.Any(p=>p.PageIdentity==identifierId)||identifierId==null)
            {
                return true;
            }
            return false;
        }

        public void DeletePage(int pageId)
        {
           var page=  GetPageById(pageId);
            _context.Pages.Remove(page);
            _context.SaveChanges();
        }

        public List<PageIndexAdminViewModel> GetAllPages()
        {
            return _context.Pages.Select(p => new PageIndexAdminViewModel()
            {
                PageID = p.PageID,
                IdentifierId = p.PageIdentity,
                PageTitle = p.Title

            }).ToList();
        }

        public FooterViewModel GetFooter()
        {
            return new FooterViewModel()
            {
                ProductsCount = ProductsCount(),
                TeachersCount = TeachersCount(),
                UserCount = UsersCount()
            };
        }

        public Page GetPageById(int pageId)
        {
            return _context.Pages.Find(pageId);
        }

        public Page GetPageByIdentifierId(int identifierId)
        {
            return _context.Pages.Single(p => p.PageIdentity == identifierId);
        }

        //public Page GetPageByTitle(string pageTitle)
        //{ 
        //    return _context.Pages.Single(p => p.Title == pageTitle);
        //}

        public int ProductsCount()
        {
            return _context.Products.Count();
        }

        public int TeachersCount()
        {
            var teachers = _productService.GetTeachers();
            return teachers.Count;
        }

        public void UpdatePage(Page page)
        {
            _context.Pages.Update(page);
            _context.SaveChanges();
        }

        public int UsersCount()
        {
            return _context.Users.Count();
        }
      

    }
}
