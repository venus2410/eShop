using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Catergories
{
    public class CategoryCreateVM
    {
        public int? ParentId { set; get; }
        public List<CategoryTranslationVM> Translations { set; get; }=new List<CategoryTranslationVM>();

    }
}
