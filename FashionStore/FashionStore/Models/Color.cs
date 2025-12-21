using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Color")]
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        [StringLength(30)]
        public string ColorHex { get; set; } // Ví dụ: #000000

        [StringLength(50)]
        public string ColorName { get; set; } // Ví dụ: Đen

        // Mối quan hệ 1-N: Một Color có thể có nhiều ProductVariant
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}