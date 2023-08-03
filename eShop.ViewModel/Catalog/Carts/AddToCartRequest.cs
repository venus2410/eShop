using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Carts
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public string UserName { get; set; }

    }
}
