using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("ProductImage")]
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(260)]
        public string ImageUrl { get; set; }

        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }

        // Mối quan hệ 1-N
        public virtual Product Product { get; set; }
    }
}