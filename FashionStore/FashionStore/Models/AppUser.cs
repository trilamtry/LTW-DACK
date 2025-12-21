using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("AppUser")]
    public class AppUser
    {
        [Key]
        [Column("UserId")]
        public int AppUserId { get; set; } // Đổi tên thuộc tính để tránh trùng với tên bảng

        [Required]
        [StringLength(120)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [StringLength(120)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Mối quan hệ 1-N: Một User có nhiều Địa chỉ
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }

        // Mối quan hệ 1-N: Một User có thể có nhiều Đơn hàng
        public virtual ICollection<Order> Orders { get; set; }

        // Mối quan hệ Nhiều-Nhiều: User <-> Role
        public virtual ICollection<Role> Roles { get; set; }
    }
}