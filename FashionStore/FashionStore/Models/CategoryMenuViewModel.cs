using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class CategoryMenuViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<CategoryItemViewModel> Categories { get; set; }
    }
}