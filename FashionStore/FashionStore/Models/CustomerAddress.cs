using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("CustomerAddress")]
    public class CustomerAddress
    {
        [Key]
        public int AddressId { get; set; }

        [Column("UserId")]
        public int AppUserId { get; set; } // Khóa ngoại tới AppUser

        [Required]
        [StringLength(200)]
        public string Line1 { get; set; } // Chi tiết địa chỉ

        [StringLength(100)]
        public string Ward { get; set; } // Phường/Xã

        [StringLength(100)]
        public string District { get; set; } // Quận/Huyện

        [StringLength(100)]
        public string Province { get; set; } // Tỉnh/Thành phố

        [StringLength(200)]
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }

        // Mối quan hệ 1-N: Một AppUser có nhiều CustomerAddress
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
    }
}