using eShop.Data.EF;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Catalog.Categories
{
    public class CatergoriesService : ICatergoriesService
    {
        private readonly EShopDbContext _context;
        public CatergoriesService(EShopDbContext context)
        {
            _context= context;
        }
        public async Task<ServiceResult<List<CatergoryVM>>> GetAll(string? languageId)
        {
            try
            {
                var language=await _context.Languages.Where(x=>x.Id==languageId).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(languageId)||language==null)
                {
                    languageId =await _context.Languages.Select(x=>x.Id).FirstOrDefaultAsync();
                }
                var query = from c in _context.Categories
                                join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                where ct.LanguageId== languageId
                                select new {c,ct };
                var catergories=await query.Select(x=>new CatergoryVM {
                    Id=x.c.Id,
                    Name=x.ct.Name,
                    ParentId=x.c.ParentId
                }).ToListAsync();

                return new ServiceResultSuccess<List<CatergoryVM>>(catergories);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<CatergoryVM>>(e.Message.ToString());
            }
        }
    }
}
