using eShop.Data.EF;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShop.Data.Entities;
using eShop.ViewModel.System.Languages;

namespace eShop.Application.Catalog.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly EShopDbContext _context;
        public CategoriesService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<bool>> Create(CategoryCreateVM model)
        {
            try
            {
                var category = new Category
                {
                    ParentId = model.ParentId ?? null,
                    CategoryTranslations = model.Translations.Select(x => new CategoryTranslation
                    {
                        Name = x.Name,
                        SeoDescription = x.SeoDescription,
                        SeoTitle = x.SeoTitle,
                        SeoAlias = x.SeoAlias,
                        LanguageId = x.LanguageId
                    }).ToList()
                };
                await _context.Categories.AddAsync(category);
                var result = await _context.SaveChangesAsync();
                if (result > 0) return new ServiceResultSuccess<bool>();
                return new ServiceResultFail<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }

        public Task<ServiceResult<bool>> Delete(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<List<CategoryVM>>> GetAll(string? languageId)
        {
            try
            {
                var language = await _context.Languages.Where(x => x.Id == languageId).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(languageId) || language == null)
                {
                    languageId = await _context.Languages.Select(x => x.Id).FirstOrDefaultAsync();
                }
                var query = from c in 
                                (from a in _context.Categories
                                join p_ct in _context.CategoryTranslations on a.ParentId equals p_ct.Id into nest
                                from p_ct in nest.DefaultIfEmpty()
                                    where p_ct.LanguageId==languageId||p_ct.LanguageId==null
                                    select new {a,p_ct})
                            join ct in _context.CategoryTranslations on c.a.Id equals ct.CategoryId
                            where ct.LanguageId == languageId
                            select new { c,ct};
                            
                var catergories = await query.Select(x => new CategoryVM
                {
                    Id = x.c.a.Id,
                    Name = x.ct.Name,
                    ParentId = x.c.a.ParentId,
                    ParentName= x.c.p_ct.Name
                }).ToListAsync();

                return new ServiceResultSuccess<List<CategoryVM>>(catergories);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<CategoryVM>>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<CategoryVM>> GetById(int categoryId, string languageId)
        {
            try
            {
                var language = await _context.Languages.Where(x => x.Id == languageId).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(languageId) || language == null)
                {
                    languageId = await _context.Languages.Select(x => x.Id).FirstOrDefaultAsync();
                }
                var query = from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                            where ct.LanguageId == languageId && ct.CategoryId == categoryId
                            select new { c, ct };
                var catergories = await query.Select(x => new CategoryVM
                {
                    Id = x.c.Id,
                    Name = x.ct.Name,
                    ParentId = x.c.ParentId,
                    Description = x.ct.SeoDescription
                }).FirstOrDefaultAsync();

                return new ServiceResultSuccess<CategoryVM>(catergories);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<CategoryVM>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<CategoryUpdateVM>> GetForUpdate(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return new ServiceResultFail<CategoryUpdateVM>();
                }
                var translations = from ct in _context.CategoryTranslations
                                   where ct.CategoryId == categoryId
                                   select new { ct };
                var catergories = new CategoryUpdateVM
                {
                    Id = categoryId,
                    ParentId = category.ParentId,
                    Translations = translations.Select(x => new CategoryTranslationVM
                    {
                        Id = x.ct.Id,
                        Name = x.ct.Name,
                        LanguageId = x.ct.LanguageId,
                        SeoDescription = x.ct.SeoDescription,
                        SeoTitle = x.ct.SeoTitle,
                        SeoAlias = x.ct.SeoAlias
                    }).ToList()
                };

                return new ServiceResultSuccess<CategoryUpdateVM>(catergories);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<CategoryUpdateVM>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<bool>> Update(CategoryUpdateVM model)
        {
            try
            {
                var category = await _context.Categories.FindAsync(model.Id);
                if (category == null)
                {
                    return new ServiceResultFail<bool>("Category not exist");
                }
                if (model.ParentId >0)
                {
                    category.ParentId = model.ParentId;
                }
                //update translations
                var translations = _context.CategoryTranslations.Where(x => x.CategoryId == model.Id).ToList();
                foreach (var translation in translations)
                {
                    var mTranslation = model.Translations.Where(x => x.Id == translation.Id).FirstOrDefault();
                    translation.Name = mTranslation.Name;
                    translation.SeoDescription = mTranslation.SeoDescription;
                    translation.SeoTitle = mTranslation.SeoTitle;
                    translation.SeoAlias = mTranslation.SeoAlias;
                }
                var result = await _context.SaveChangesAsync();
                if (result > 0) return new ServiceResultSuccess<bool>();
                return new ServiceResultFail<bool>("Fail to update");
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }
    }
}
