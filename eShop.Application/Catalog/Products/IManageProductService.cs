﻿using eShop.ViewModel.Catalog.DTOs;
using eShop.ViewModel.Catalog.Products.DTOs;
using eShop.ViewModel.Catalog.Products.DTOs.Manage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedStock);
        Task AddViewCount(int productId);
        Task<PageResult<ProductViewModel>> GetByPaging(ManageProductPagingRequest request);
        Task<int> AddImages(int productId, List<IFormFile> files);

        Task<int> RemoveImages(int imageId);

        Task<int> UpdateImage(int imageId, string caption, bool isDefault);

        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
