using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("CategoryGroup")]
    public class CategoryGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [StringLength(40)]
        public string GroupCode { get; set; } // Mã nhóm (ví dụ: ao, quan, phu-kien)

        [StringLength(120)]
        public string GroupName { get; set; } // Tên hiển thị (ví dụ: Áo, Quần, Phụ Kiện)

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        // Mối quan hệ 1-N: Một Group có nhiều Category
        public virtual ICollection<Category> Categories { get; set; }
    }
}