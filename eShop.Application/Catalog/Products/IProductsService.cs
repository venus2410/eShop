using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.ProductImages;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IProductsService
    {
        Task<ServiceResult<bool>> Create(ProductCreateRequest request);
        Task<ServiceResult<bool>> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<ServiceResult<ProductVM>> GetById(int productId, string languageId);
        Task<ServiceResult<ProductUpdateRequest>> GetForUpdate(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedStock);
        Task AddViewCount(int productId);
        Task<ServiceResult<PageResult<ProductVM>>> GetByPaging(GetManageProductPagingRequest request);
        Task<ServiceResult<int>> AddImage(int productId, ProductImageCreateRequest request);
        Task<ServiceResult<int>> RemoveImages(List<int> imageIds);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<ServiceResult<List<ProductImageViewModel>>> GetListImage(int productId);
        Task<PageResult<ProductVM>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
        Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take);
        Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take);
        Task<ServiceResult<List<TranslationOfProduct>>> GetProductTranslation(int productId);
    }
}
