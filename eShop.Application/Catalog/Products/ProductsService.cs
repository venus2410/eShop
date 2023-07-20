using eShop.ViewModel.Catalog.Common;
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
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Localization.Internal;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.Application.Catalog.Products
{
    public class ProductsService : IProductsService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductsService(EShopDbContext context, IStorageService storageService)
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

        public async Task<ServiceResult<bool>> Create(ProductCreateRequest request)
        {
            try
            {
                //create product translations for product
                var productTranslations = new List<ProductTranslation>();
                foreach (var translation in request.Translations)
                {
                    productTranslations.Add(
                    new ProductTranslation()
                    {
                        Name = translation.Name ?? "N/A",
                        Description = translation.Description ?? "N/A",
                        Details = translation.Details ?? "N/A",
                        SeoDescription = translation.SeoDescription ?? "N/A",
                        SeoTitle = translation.SeoTitle ?? "N/A",
                        SeoAlias = translation.SeoAlias ?? "N/A",
                        LanguageId = translation.LanguageId ?? "N/A"
                    });
                }
                //create product in category for product
                var pics = new List<ProductInCategory>
                {
                    new ProductInCategory
                    {
                        CategoryId=request.CategoryId
                    }
                };

                var product = new Product()
                {
                    Price = request.Prices.Price,
                    OriginalPrice = request.Prices.OriginalPrice,
                    Stock = request.Stock,
                    ViewCount = 0,
                    DateCreated = DateTime.Now,
                    ProductTranslations = productTranslations,
                    IsFeatured = request.IsFeatured ?? false,
                    ProductInCategories = pics
                };

                //save image

                if (request.ThumbnailImage != null)
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage()
                        {
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
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ServiceResultSuccess<bool>();
                }
                return new ServiceResultFail<bool>("Có lỗi trong quá trình xử lý");
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }

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

        public async Task<ServiceResult<ProductVM>> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new ServiceResultFail<ProductVM>("Không tìm thấy");
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            productTranslation ??= await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId);
            var productImages = _context.ProductImages.Where(x => x.ProductId == productId).OrderByDescending(x => x.IsDefault).Select(x => x.ImagePath).ToList();
            var model = new ProductVM
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                LanguageId = languageId,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Name = productTranslation.Name ?? null,
                Description = productTranslation.Description ?? null,
                Details = productTranslation.Details ?? null,
                SeoAlias = productTranslation.SeoAlias ?? null,
                SeoDescription = productTranslation.SeoDescription ?? null,
                SeoTitle = productTranslation.SeoTitle ?? null,
                ThumbnailImage = productImages.FirstOrDefault(),
                OtherImages = productImages.Skip(1).Take(productImages.Count - 1).ToList()
            };
            return new ServiceResultSuccess<ProductVM>(model);
        }

        public async Task<ServiceResult<PageResult<ProductVM>>> GetByPaging(GetManageProductPagingRequest request)
        {
            try
            {
                //select join
                var query = from p in _context.Products
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId into ppt
                            from pt in ppt.DefaultIfEmpty()
                            select new { p, pt };
                //select new { p, pt };
                //filter

                if (!string.IsNullOrEmpty(request.LanguageId))
                {
                    query = query.Where(x => x.pt.LanguageId == request.LanguageId);
                }
                if (!string.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.pt.Name.Contains(request.Keyword));
                if (request.CategoryId != null)
                {
                    var matchedCategories = from c in _context.Categories
                                            where c.Id == request.CategoryId || c.ParentId == request.CategoryId
                                            select new { c };
                    query = query.Where(x => _context.ProductInCategories.Any(y => y.ProductId == x.p.Id && matchedCategories.Any(z => z.c.Id == y.CategoryId)));
                }

                //paging
                int totalRecord = await query.CountAsync();
                //error at take???
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new ProductVM()
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
                foreach (var d in data)
                {
                    var images = _context.ProductImages.Where(x => x.ProductId == d.Id).OrderByDescending(x => x.IsDefault).Select(x => x.ImagePath).ToList();
                    d.ThumbnailImage = images.FirstOrDefault() ?? null;
                    if (images.Count >= 2)
                    {
                        d.OtherImages = images.Skip(1).Take(images.Count() - 1).ToList();
                    }
                }

                // return data
                var result = new PageResult<ProductVM>()
                {
                    Items = data,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecord
                };

                return new ServiceResultSuccess<PageResult<ProductVM>>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<PageResult<ProductVM>>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<bool>> Update(ProductUpdateRequest request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.Id);
                foreach (var translation in request.Translations)
                {
                    var productTranslation = await _context.ProductTranslations.Where(x => x.ProductId == request.Id && x.LanguageId == translation.LanguageId).FirstOrDefaultAsync();
                    if (product == null || productTranslation == null) { throw new EShopException($"Cannot find a product with id: {request.Id}"); }

                    productTranslation.Name = translation.Name;
                    productTranslation.SeoAlias = translation.SeoAlias;
                    productTranslation.SeoDescription = translation.SeoDescription;
                    productTranslation.SeoTitle = translation.SeoTitle;
                    productTranslation.Description = translation.Description;
                    productTranslation.Details = translation.Details;
                }
                //update categoryId
                var productInCategory = await _context.ProductInCategories.Where(x => x.ProductId == request.Id).FirstOrDefaultAsync();
                if (productInCategory != null)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                var newProductInCategory = new ProductInCategory
                {
                    CategoryId = request.CategoryId,
                    ProductId = request.Id
                };
                _context.ProductInCategories.Add(newProductInCategory);

                var result = await _context.SaveChangesAsync();
                if (result <= 0) return new ServiceResultFail<bool>("Cập nhật thất bại");
                return new ServiceResultSuccess<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }

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

        public async Task<ServiceResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) { return new ServiceResultFail<int>("Không tìm thấy sản phẩm"); }
            if (request.ImageFiles == null) { return new ServiceResultFail<int>("Không có ảnh"); }
            var productImages = new List<ProductImage>();
            foreach(var file in request.ImageFiles)
            {
                var image= new ProductImage()
                {
                    ProductId = productId,
                    ImagePath = await SaveFile(file),
                    Caption = request.Caption,
                    IsDefault = request.IsDefault,
                    DateCreated = DateTime.Now,
                    SortOrder = request.SortOrder,
                    FileSize = file.Length,
                };
                productImages.Add(image);
            }
                 
            await _context.ProductImages.AddRangeAsync(productImages);
            var result= await _context.SaveChangesAsync();
            return new ServiceResult<int> { 
                IsSucceed=result>0,
                Data= result,
                Errors=result>0?null:"Thêm ảnh không thành công"
            };
        }
        public async Task<ServiceResult<int>> RemoveImages(List<int> imageIds)
        {
            var productImages = await _context.ProductImages.Where(x=>imageIds.Contains(x.Id)).ToListAsync();
            var filePaths=productImages.Select(x=>x.ImagePath).ToList();
            //delete in database
            _context.ProductImages.RemoveRange(productImages);
            //delete in folder
            await _storageService.DeleteFilesAsync(filePaths);

            var result = await _context.SaveChangesAsync();
            return new ServiceResult<int>()
            {
                IsSucceed=result>0,
                Data= result,
                Errors=result>0?"":"Xóa ảnh không thành công"
            };
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
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) { throw new EShopException($"Cannot find an image with id {imageId}"); }
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
        public async Task<ServiceResult<List<ProductImageViewModel>>> GetListImage(int productId)
        {
            var images= await _context.ProductImages.Where(x => x.ProductId == productId)
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
            if (images == null) return new ServiceResultFail<List<ProductImageViewModel>>("Không tìm thấy ảnh");
            return new ServiceResultSuccess<List<ProductImageViewModel>>(images);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return _storageService.GetFileUrl(fileName);
        }
        public async Task<PageResult<ProductVM>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //filter
            if (request.CategoryId.HasValue && request.CategoryId > 0)
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            //paging
            int totalRecord = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVM()
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
            var result = new PageResult<ProductVM>()
            {
                Items = data,
                TotalRecords = totalRecord
            };

            return result;
        }

        public async Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take)
        {
            try
            {
                var query = from p in _context.Products
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                            join pi in _context.ProductImages on p.Id equals pi.ProductId
                            where pt.LanguageId == languageId && p.IsFeatured == true
                            select new { p, pt, pi };
                var result = await query.Take(take).Select(x => new ProductVM
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
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }
                ).ToListAsync();
                return new ServiceResultSuccess<List<ProductVM>>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<ProductVM>>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take)
        {
            try
            {
                var query = from p in _context.Products
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                            join pi in _context.ProductImages on p.Id equals pi.ProductId
                            where pt.LanguageId == languageId && p.IsFeatured == true
                            select new { p, pt, pi };
                var result = await query.OrderByDescending(x => x.p.DateCreated).Take(take).Select(x => new ProductVM
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
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }
                ).ToListAsync();
                return new ServiceResultSuccess<List<ProductVM>>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<ProductVM>>(e.Message.ToString());
            }
        }
#nullable enable
        public async Task<ServiceResult<List<TranslationOfProduct>>> GetProductTranslation(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return new ServiceResultFail<List<TranslationOfProduct>>("Không tìm thấy bản dịch");
                }
                var query = from pt in _context.ProductTranslations
                            join l in _context.Languages on pt.LanguageId equals l.Id
                            where pt.ProductId == productId
                            select new { pt, l };
                var translations = await query.Select(
                    x => new TranslationOfProduct
                    {
                        Id = x.pt.Id,
                        LanguageId = x.pt.LanguageId,
                        Name = x.pt.Name,
                        Description = x.pt.Description,
                        Details = x.pt.Details,
                        SeoDescription = x.pt.SeoDescription,
                        SeoAlias = x.pt.SeoAlias,
                        SeoTitle = x.pt.SeoTitle,
                        LanguageName = x.l.Name
                    }
                   ).ToListAsync();
                return new ServiceResultSuccess<List<TranslationOfProduct>>(translations);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<List<TranslationOfProduct>>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<ProductUpdateRequest>> GetForUpdate(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) { return new ServiceResultFail<ProductUpdateRequest>("Không tìm thấy sản phẩm"); }
                //translations already made
                var query = from pt in _context.ProductTranslations
                            join l in _context.Languages on pt.LanguageId equals l.Id
                            where pt.ProductId == productId
                            select new { pt, l };
                var translations = await query.Select(x => new TranslationOfProduct
                {
                    Id = x.pt.Id,
                    LanguageId = x.pt.LanguageId,
                    LanguageName = x.l.Name,
                    Name = x.pt.Name ?? "",
                    Description = x.pt.Description ?? "",
                    Details = x.pt.Details ?? "",
                    SeoDescription = x.pt.SeoDescription ?? "",
                    SeoTitle = x.pt.SeoTitle ?? "",
                    SeoAlias = x.pt.SeoAlias ?? ""
                }).ToListAsync();
                //translation of language haven't made
                var defaultInfor = ProductSetting.DefaultProductInfor;
                var unmadeTranslations = await (from l in _context.Languages
                                                where !query.Any(x => x.pt.ProductId == productId && x.pt.LanguageId == l.Id)
                                                select new { l }).Select(x => new ProductTranslation
                                                {
                                                    ProductId = productId,
                                                    Name = defaultInfor,
                                                    Description = defaultInfor,
                                                    Details = defaultInfor,
                                                    SeoDescription = defaultInfor,
                                                    SeoTitle = defaultInfor,
                                                    SeoAlias = defaultInfor,
                                                    LanguageId = x.l.Id
                                                }).ToListAsync();
                _context.ProductTranslations.AddRange(unmadeTranslations);
                _context.SaveChanges();
                translations.AddRange((from u in unmadeTranslations
                                       join l in _context.Languages on u.LanguageId equals l.Id
                                       select new { u, l }
                    ).Select(x => new TranslationOfProduct
                    {
                        Id = x.u.Id,
                        LanguageId = x.u.LanguageId,
                        LanguageName = x.l.Name,
                        Name = x.u.Name,
                        Description = x.u.Description,
                        Details = x.u.Details,
                        SeoDescription = x.u.SeoDescription,
                        SeoTitle = x.u.SeoTitle,
                        SeoAlias = x.u.SeoAlias
                    }).ToList());


                var category = await _context.ProductInCategories.Where(x => x.ProductId == productId).Select(x => x.CategoryId).FirstOrDefaultAsync();
                var result = new ProductUpdateRequest
                {
                    Id = productId,
                    Translations = translations,
                    CategoryId = category
                };
                return new ServiceResultSuccess<ProductUpdateRequest>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<ProductUpdateRequest>(e.Message.ToString());
            }

        }
    }
}
