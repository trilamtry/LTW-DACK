using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class ProductListItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Slug { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }

    
}