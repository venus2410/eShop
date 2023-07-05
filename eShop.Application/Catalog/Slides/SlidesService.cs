using eShop.Data.EF;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Slides;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Slides
{
    public class SlidesService:ISlidesService
    {
        private readonly EShopDbContext _context;
        public SlidesService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<List<SlideVM>>> GetAll()
        {
            try
            {
                var slides = await _context.Slides.OrderBy(x=>x.SortOrder)
                    .Select(x=>new SlideVM
                {
                    Id= x.Id,
                    Name= x.Name,
                    Description= x.Description,
                    Url= x.Url,
                    Image= x.Image,
                    SortOrder= x.SortOrder
                }).ToListAsync();
                

                return new ServiceResultSuccess<List<SlideVM>>(slides);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<SlideVM>>(e.Message.ToString());
            }
        }
    }
}
