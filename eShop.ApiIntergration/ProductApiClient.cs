﻿using eShop.Utilities.Constants;
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
            for (int i = 0; i < request.Translations.Count; i++)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Name) ? "" : request.Translations[i].Name.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.Name));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Description) ? "" : request.Translations[i].Description.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.Description));

                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].Details) ? "" : request.Translations[i].Details.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.Details));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoDescription) ? "" : request.Translations[i].SeoDescription.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.SeoDescription));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoTitle) ? "" : request.Translations[i].SeoTitle.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.SeoTitle));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].SeoAlias) ? "" : request.Translations[i].SeoAlias.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.SeoAlias));
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Translations[i].LanguageId) ? "" : request.Translations[i].LanguageId.ToString()), nameof(Translation) + $"s[{i}]." + nameof(Translation.LanguageId));
            }

            requestContent.Add(new StringContent(languageId), "languageId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResultSuccess<bool>();
            }
            return new ServiceResultFail<bool>(response.StatusCode.ToString());
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

        public async Task<ServiceResult<List<Translation>>> GetProductTranslation(int productId)
        {
            string url = baseUrl + $"/{productId}/translations";
            return await _baseApiClient.GetAllAsync<List<Translation>>(url);
        }

        public async Task<ServiceResult<bool>> Update(ProductUpdateRequest request)
        {
            return await _baseApiClient.PutAsync<bool, ProductUpdateRequest>(baseUrl, request.Id.ToString(), request);
        }
    }
}
