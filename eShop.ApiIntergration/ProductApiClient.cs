using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.ProductImages;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.ApiIntergration
{
    public class ProductApiClient : IProductApiClient
    {
        private const string baseUrl = "/api/products";
        private readonly IBaseApiClient _baseApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ProductApiClient(IBaseApiClient baseApiClient, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _baseApiClient = baseApiClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ServiceResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
            .Session
            .GetString(AppSetting.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[AppSetting.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();
            if(request.ImageFiles!= null)
            {
                foreach(var image in request.ImageFiles)
                {
                    byte[] data;
                    using (var br = new BinaryReader(image.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)image.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, nameof(ProductImageCreateRequest.ImageFiles), image.FileName);
                }
            }
            

            requestContent.Add(new StringContent(request.Caption), nameof(ProductImageCreateRequest.Caption));
            requestContent.Add(new StringContent(request.IsDefault.ToString()), nameof(ProductImageCreateRequest.IsDefault));
            requestContent.Add(new StringContent(request.SortOrder.ToString()), nameof(ProductImageCreateRequest.SortOrder));

            var response = await client.PostAsync($"/api/products/{productId}/images", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<int>>(body);
        }

        public async Task<ServiceResult<bool>> Create(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
            .Session
            .GetString(AppSetting.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(AppSetting.CurrentLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[AppSetting.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Prices.Price.ToString()), "Prices.Price");
            requestContent.Add(new StringContent(request.Prices.OriginalPrice.ToString()), "Prices.OriginalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            var translations = nameof(ProductCreateRequest.Translations);
            for (int i = 0; i < request.Translations.Count; i++)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Name) ? "" : request.Translations[i].Name.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.Name));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Description) ? "" : request.Translations[i].Description.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.Description));

                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Details) ? "" : request.Translations[i].Details.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.Details));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoDescription) ? "" : request.Translations[i].SeoDescription.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.SeoDescription));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoTitle) ? "" : request.Translations[i].SeoTitle.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.SeoTitle));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoAlias) ? "" : request.Translations[i].SeoAlias.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.SeoAlias));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].LanguageId) ? "" : request.Translations[i].LanguageId.ToString()), translations + $"[{i}]." + nameof(TranslationOfProduct.LanguageId));
            }

            requestContent.Add(new StringContent(languageId), "languageId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceResult<bool>>(body);
        }

        public async Task<ServiceResult<ProductVM>> GetById(int productId, string languageId)
        {
            return await _baseApiClient.GetByIdAsync<ProductVM>(baseUrl,productId.ToString(),languageId);
        }

        public async Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take)
        {
            string url = baseUrl + $"/featured/{languageId}/{take}";
            return await _baseApiClient.GetGeneralAsync<List<ProductVM>>(url);
        }

        public Task<ServiceResult<ProductUpdateRequest>> GetForUpdate(int productId)
        {
            var url = $"{baseUrl}/getforupdate/{productId}";
            return _baseApiClient.GetGeneralAsync<ProductUpdateRequest>(url);
        }

        public async Task<ServiceResult<List<ProductImageViewModel>>> GetImages(int productId)
        {
            var url = $"{baseUrl}/{productId}/images";
            return await _baseApiClient.GetGeneralAsync<List<ProductImageViewModel>>(url);
        }

        public async Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take)
        {
            string url = baseUrl + $"/latest/{languageId}/{take}";
            return await _baseApiClient.GetGeneralAsync<List<ProductVM>>(url);
        }

        public async Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request)
        {
            string getURI = baseUrl + $"/paging?" + $"{nameof(request.PageIndex)}={request.PageIndex}&" + $"{nameof(request.PageSize)}={request.PageSize}&" + $"{nameof(request.Keyword)}={request.Keyword}&" + $"{nameof(request.LanguageId)}={request.LanguageId}&" + $"{nameof(request.CategoryId)}={request.CategoryId}&" + $"{nameof(request.OrderBy)}={request.OrderBy}";

            //Dictionary<string,string> dictionary=(Dictionary<string, string>) request;
            //QueryHelpers.AddQueryString(getURI, dictionary);
            return await _baseApiClient.GetGeneralAsync<PageResult<ProductVM>>(getURI);
        }

        public async Task<ServiceResult<List<TranslationOfProduct>>> GetProductTranslation(int productId)
        {
            string url = baseUrl + $"/{productId}/translations";
            return await _baseApiClient.GetGeneralAsync<List<TranslationOfProduct>>(url);
        }

        public async Task<ServiceResult<bool>> RemoveImages(List<int> imageIds, int productId)
        {
            var url = $"{baseUrl}/{productId}/images?";
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach(var imageId in imageIds)
            {
                queryString.Add("imageIds",imageId.ToString());
            }
            return await _baseApiClient.DeleteListAsync<bool>(url + queryString.ToString());
        }

        public async Task<ServiceResult<bool>> Update(ProductUpdateRequest request)
        {
            return await _baseApiClient.PutAsync<bool, ProductUpdateRequest>(baseUrl, request.Id.ToString(), request);
        }
    }
}
