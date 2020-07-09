using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.Product
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double OriginalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CategoryID { get; set; }
    }
}
