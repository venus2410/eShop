using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Slides;
using eShop.ViewModel.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public class SlideApiClient : ISlideApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/slides";
        public SlideApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }
        public async Task<ServiceResult<List<SlideVM>>> GetAll()
        {
            return await _baseApiClient.GetAllAsync<List<SlideVM>>(baseURL);
        }
    }
}
