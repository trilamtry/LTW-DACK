using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string SKU { get; set; }

        [Required]
        [StringLength(180)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(120)]
        public string Slug { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        [Required]
        public int CategoryId { get; set; }

        // Navigation Properties

        // Mối quan hệ: Một Product thuộc về một Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // Mối quan hệ: Một Product có nhiều Variants (Size, Color)
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }

        // Mối quan hệ: Một Product có nhiều hình ảnh
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}