using FashionStore.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("ProductReview")] // Thêm dòng này để khớp với tên bảng trong SQL của bạn
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [StringLength(30)]
        public string ReviewCode { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; }
    }
}