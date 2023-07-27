using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.Catergories
{
    public class CategoryTranslationVM
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        [Required]
        public string LanguageId { set; get; }
        [Required]
        public string SeoAlias { set; get; }
    }
}
