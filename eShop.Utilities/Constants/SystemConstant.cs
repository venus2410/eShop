using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace eShop.Utilities.Constants
{
    public class SystemConstant
    {
        public const string MainConnectionString = "EShopDatabase";
        public class AppSetting
        {
            public const string CurrentLanguageId = "CurrentLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
            public const string Bearer = "Bearer";
        }
        public class UserSetting
        {
            public const string DefaultRoleNameForUser = "user";
        }
        public class Currency
        {
        }
        public class ProductSetting
        {
            public const int NumberOfFeaturedProducts = 16;
            public const int NumberOfLatestProducts = 6;
            public const int NumberOfProductWebAppTable = 9;
            public const string DefaultProductInfor = "Default Infor";
        }
    }
}
