using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Size")]
    public class Size
    {
        [Key]
        public int SizeId { get; set; }

        [Required]
        [StringLength(10)]
        public string SizeCode { get; set; } // Ví dụ: S, M, L

        public int SortOrder { get; set; }

        // Mối quan hệ 1-N: Một Size có thể có nhiều ProductVariant
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}