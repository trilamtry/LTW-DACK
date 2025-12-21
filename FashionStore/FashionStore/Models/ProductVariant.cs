using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace FashionStore.Models
{
    [Table("ProductVariant")]
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }

        public decimal Price { get; set; } // Giá bán của biến thể
        public int Stock { get; set; }     // Tồn kho của biến thể

        [StringLength(50)]
        public string SKU { get; set; }

        // Thuộc tính điều hướng
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
        public virtual Color Color { get; set; }
    }
}