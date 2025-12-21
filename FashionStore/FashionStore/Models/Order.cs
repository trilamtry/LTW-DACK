using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Order")] // Quan trọng: Đặt tên bảng là [Order]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [StringLength(30)]
        public string OrderCode { get; set; }

        [Column("UserId")]
        public int? AppUserId { get; set; } // Khóa ngoại, cho phép NULL (Guest checkout)

        [Required]
        [StringLength(120)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(220)]
        public string AddressLine { get; set; }

        [StringLength(240)]
        public string MessageCard { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // PENDING/PAID/CANCELLED

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        // Mối quan hệ
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}