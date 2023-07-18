using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResultSuccess<bool>();
            }
            return new ServiceResultFail<bool>(response.StatusCode.ToString());
        }

        public async Task<ServiceResult<ProductVM>> GetById(int productId, string languageId)
        {
            return await _baseApiClient.GetByIdAsync<ProductVM>(baseUrl,productId.ToString(),languageId);
        }

        public async Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take)
        {
            string url = baseUrl + $"/featured/{languageId}/{take}";
            return await _baseApiClient.GetAllAsync<List<ProductVM>>(url);
        }

        public async Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take)
        {
            string url = baseUrl + $"/latest/{languageId}/{take}";
            return await _baseApiClient.GetAllAsync<List<ProductVM>>(url);
        }

        public async Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request)
        {
            string getURI = baseUrl + $"/paging?" + $"{nameof(request.PageIndex)}={request.PageIndex}&" + $"{nameof(request.PageSize)}={request.PageSize}&" + $"{nameof(request.Keyword)}={request.Keyword}&" + $"{nameof(request.LanguageId)}={request.LanguageId}&" + $"{nameof(request.CategoryId)}={request.CategoryId}";
            //Dictionary<string,string> dictionary=(Dictionary<string, string>) request;
            //QueryHelpers.AddQueryString(getURI, dictionary);
            return await _baseApiClient.GetAllAsync<PageResult<ProductVM>>(getURI);
        }

        public async Task<ServiceResult<List<TranslationOfProduct>>> GetProductTranslation(int productId)
        {
            string url = baseUrl + $"/{productId}/translations";
            return await _baseApiClient.GetAllAsync<List<TranslationOfProduct>>(url);
        }

        public async Task<ServiceResult<bool>> Update(ProductUpdateRequest request)
        {
            return await _baseApiClient.PutAsync<bool, ProductUpdateRequest>(baseUrl, request.Id.ToString(), request);
        }
    }
}
