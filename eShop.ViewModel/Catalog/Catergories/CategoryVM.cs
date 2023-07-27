using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Catergories
{
    public class CategoryVM
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int? ParentId { set; get; }
        public string ParentName { set; get; }
        public string Description { set; get; }
    }
}
