using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.ViewModel.Catalog.Carts;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Carts
{
    public class CartsService : ICartsService
    {
        readonly EShopDbContext _context;
        readonly UserManager<AppUser> _userManager;
        public CartsService(EShopDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ServiceResult<bool>> AddProduct(AddToCartRequest request)
        {
            try
            {
                //find user by username
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new ServiceResultFail<bool>("User not found");
                }
                //find product and stock
                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null) { return new ServiceResultFail<bool>("Product not found"); }
                if (product.Stock < request.Quantity) { return new ServiceResultFail<bool>("Product out of stock"); }

                var cart = new Cart
                {
                    ProductId = product.Id,
                    Quantity=request.Quantity,
                    Price=product.Price,
                    UserId= user.Id,
                    DateCreated= DateTime.UtcNow
                };
                await _context.Carts.AddAsync(cart);
                var result= await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ServiceResultSuccess<bool>();
                }
                return new ServiceResultFail<bool>("Fail to add product to card");
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }
    }
}
