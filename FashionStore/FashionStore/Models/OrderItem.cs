using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int VariantId { get; set; } // Biến thể sản phẩm (FK tới ProductVariant, nếu cần)

        [StringLength(180)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string SizeName { get; set; }

        [StringLength(50)]
        public string ColorName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Mối quan hệ 1-N: Một Order có nhiều OrderItem
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // Mối quan hệ với ProductVariant (Nếu muốn truy cập chi tiết biến thể)
        // public virtual ProductVariant Variant { get; set; } 
    }
}