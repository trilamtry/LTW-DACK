using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("Role")] // Tên bảng là [Role]
    public class Role
    {
        [Key]
        [Column("Rid")]
        public int RoleId { get; set; }

        [Column("RName")]
        [StringLength(20)]
        public string RoleName { get; set; }

        // Mối quan hệ Nhiều-Nhiều với AppUser (Qua bảng trung gian UserRole)
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}