using eShop.Data.EF;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _context;
        public LanguageService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<LanguageVM>>> GetLanguageVMsAsync()
        {
            try
            {
                var languages = await _context.Languages.Select(l => new LanguageVM
                {
                    Id = l.Id,
                    Name = l.Name,
                    IsDefault = l.IsDefault
                }).ToListAsync();
                return new ServiceResultSuccess<List<LanguageVM>>(languages);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<LanguageVM>>(e.Message.ToString());
            }
            
        }
    }
}
