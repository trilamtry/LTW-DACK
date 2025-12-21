using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Category")]
    public class Category
    {
        public int CategoryId { get; set; }

        public int GroupId { get; set; }

        [Column("CatSlug")]
        public string CategorySlug { get; set; }

        [Column("CatName")]
        public string CategoryName { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        // Quan hệ
        public virtual CategoryGroup CategoryGroup { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}