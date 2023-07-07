using eShop.ApiIntergration;
using eShop.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using eShop.Utilities.Constants;
using static eShop.Utilities.Constants.SystemConstant;
using Microsoft.Extensions.Configuration;

namespace eShop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly ISharedCultureLocalizer _loc;
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ISlideApiClient slideApiClient, ISharedCultureLocalizer loc, IProductApiClient productApiClient, IConfiguration configuration)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _loc = loc;
            _productApiClient = productApiClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var baseAddress=_configuration[AppSetting.BaseAddress];
            var slides = _slideApiClient.GetAll().Result.Data;
            var featuredProducts = _productApiClient.GetFeaturedProduct(culture, ProductSetting.NumberOfFeaturedProducts).Result.Data;
            var latestProducts= _productApiClient.GetLatestProduct(culture, ProductSetting.NumberOfLatestProducts).Result.Data;
            var homeVM = new HomeViewModel()
            {
                BaseAddress =baseAddress ,
                Slides=slides,
                FeaturedProducts=featuredProducts,
                LatestProducts=latestProducts
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
