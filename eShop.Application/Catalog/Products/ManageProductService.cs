﻿using eShop.ViewModel.Catalog.Common;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using eShop.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using eShop.ViewModel.Catalog.Products;
using System.Collections;
using System.Xml.Linq;
using eShop.ViewModel.Catalog.ProductImages;

namespace eShop.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }



        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount++;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation> {
                   new ProductTranslation()
                   {
                       Name= request.Name,
                       Description= request.Description,
                       Details= request.Details,
                       SeoDescription= request.SeoDescription,
                       SeoTitle=request.SeoTitle,
                       SeoAlias=request.SeoAlias,
                       LanguageId=request.LanguageId
                   }
               }
            };

            //save image

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>() {
                    new ProductImage() {
                        Caption="Thumbnail image",
                        DateCreated=DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath=await this.SaveFile(request.ThumbnailImage),
                        IsDefault=true,
                        SortOrder=1
                    }

                };
            }


            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            var images = _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            if (product == null) return null;
            var model = new ProductViewModel();
            if (product != null)
            {
                model.Id = product.Id;
                model.DateCreated = product.DateCreated;
                model.LanguageId = languageId;
                model.OriginalPrice = product.OriginalPrice;
                model.Price = product.Price;
                model.Stock = product.Stock;
                model.ViewCount = product.ViewCount;
            }
            if (productTranslation != null)
            {
                model.Name = productTranslation.Name ?? null;
                model.Description = productTranslation.Description ?? null;
                model.Details = productTranslation.Details ?? null;
                model.SeoAlias = productTranslation.SeoAlias ?? null;
                model.SeoDescription = productTranslation.SeoDescription ?? null;
                model.SeoTitle = productTranslation.SeoTitle ?? null;
            };
            return model;
        }

        public async Task<PageResult<ProductViewModel>> GetByPaging(string languageId, GetManageProductPagingRequest request)
        {
            //select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            if (request.CategoryIds.Count() > 0)
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
            //paging
            int totalRecord = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            // return data
            var result = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRecord
            };

            return result;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.Where(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId).FirstOrDefaultAsync();
            if (product == null || productTranslation == null) { throw new EShopException($"Cannot find a product with id: {request.Id}"); }

            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;

            //save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.IsDefault);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }



            return await _context.SaveChangesAsync();

        }
        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) { throw new EShopException($"Cannot find a product with id: {productId}"); }
            product.Price = newPrice;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedStock)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) { throw new EShopException($"Cannot find a product with id: {productId}"); }
            product.Stock = addedStock;

            return await _context.SaveChangesAsync() > 0;
        }

        // FOR IMAGES

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) { return 0; }
            if (request.ImageFile == null) { return 0; }
            var productImage = new ProductImage()
            {
                ProductId = productId,
                ImagePath = await SaveFile(request.ImageFile),
                Caption = request.Caption,
                IsDefault = request.IsDefault,
                DateCreated = DateTime.Now,
                SortOrder = request.SortOrder,
                FileSize = request.ImageFile.Length,
            };
            await _context.ProductImages.AddAsync(productImage);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) { return -1; }

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }
        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) { throw new EShopException($"Cannot find an image with id {imageId}"); }
            if (request.ImageFile == null) { throw new EShopException("There is no image file inserted"); }

            productImage.ImagePath = await SaveFile(request.ImageFile);
            productImage.FileSize = request.ImageFile.Length;

            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image= await _context.ProductImages.FindAsync(imageId);
            if(image == null) { throw new EShopException($"Cannot find an image with id {imageId}"); }
            var model = new ProductImageViewModel()
            {
                Id = image.Id,
                ProductId = image.ProductId,
                ImagePath = image.ImagePath,
                Caption = image.Caption,
                IsDefault = image.IsDefault,
                DateCreated = image.DateCreated,
                SortOrder = image.SortOrder,
                FileSize = image.FileSize
            };
            return model;
        }
        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(x => new ProductImageViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ImagePath = x.ImagePath,
                    Caption = x.Caption,
                    IsDefault = x.IsDefault,
                    DateCreated = x.DateCreated,
                    SortOrder = x.SortOrder,
                    FileSize = x.FileSize
                }).ToListAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
